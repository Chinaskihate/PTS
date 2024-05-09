﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PTS.Backend.Exceptions.Common;
using PTS.Backend.Service.IService;
using PTS.Contracts.Test;
using PTS.Contracts.Tests.Dto;
using PTS.Contracts.Versions.Dto;
using PTS.Persistence.DbContexts;
using PTS.Persistence.Models.Tests;
using Serilog;
using System;
using System.Runtime.Serialization.Json;
using static System.Net.Mime.MediaTypeNames;

namespace PTS.Backend.Service;
public class TestService(
    IDbContextFactory<TestDbContext> dbContextFactory,
    ITaskVersionProxyService versionService,
    IMapper mapper) : ITestService
{
    private readonly IDbContextFactory<TestDbContext> _dbContextFactory = dbContextFactory;
    private readonly ITaskVersionProxyService _versionService = versionService;
    private readonly IMapper _mapper = mapper;

    public async Task<TestDto> CreateAsync(CreateTestRequest dto, string userId)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var versions = new List<VersionForTestDto>();
        var test = new Test()
        {
            Name = dto.Name,
            Description = dto.Description,
            IsEnabled = dto.IsEnabled,
            AllowedExecutionTimeInSec = dto.AllowedExecutionTimeInSec,
            CreatorId = userId,
            IsAutoGenerated = dto.IsAutoGenerated,
        };

        foreach (var item in dto.Versions)
        {
            var version = await _versionService.GetAsync(item.TaskId, item.VersionId);
            if (!version.IsTaskEnabled)
            {
                throw new EntityDisabledException($"Version {item.VersionId} with task {item.TaskId} is disabled");
            }
            versions.Add(version);
        }

        if (versions.IsNullOrEmpty())
        {
            throw new NotFoundException("No task versions found");
        }

        context.Tests.Add(test);
        await context.SaveChangesAsync();

        foreach (var item in versions)
        {
            var testTaskVersion = new TestTaskVersion
            {
                Test = test,
                TaskVersionId = item.Id,
                TaskId = item.TaskId
            };
            context.TestTaskVersions.Add(testTaskVersion);
        }

        await context.SaveChangesAsync();

        var result = _mapper.Map<TestDto>(test);
        result.TaskVersions = versions;

        return result;
    }

    public async Task<TestDto> EditAsync(EditTestRequest dto, int id)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var test = await context.Tests
            .Include(t => t.TestTaskVersions)
            .FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new NotFoundException($"Test with {id} id not found");

        if (dto.IsEnabled.HasValue)
        {
            test.IsEnabled = dto.IsEnabled.Value;
        }
        if (dto.AllowedExecutionTimeInSec.HasValue)
        {
            test.AllowedExecutionTimeInSec = dto.AllowedExecutionTimeInSec.Value;
        }
        if (dto.Name != null)
        {
            test.Name = dto.Name;
        }
        if (dto.Description != null)
        {
            test.Description = dto.Description;
        }

        await context.SaveChangesAsync();

        var versions = new List<VersionForTestDto>();
        var result = _mapper.Map<TestDto>(test);
        foreach (var testTaskVersion in test.TestTaskVersions)
        {
            versions.Add(await _versionService.GetAsync(testTaskVersion.TaskId, testTaskVersion.TaskVersionId));
        }

        result.TaskVersions = versions;
        return result;
    }

    public async Task<TestDto> Get(int id)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var test = await context.Tests
            .Include(t => t.TestTaskVersions)
            .FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new NotFoundException($"Test with {id} id not found");


        var result = _mapper.Map<TestDto>(test);
        var versions = new List<VersionForTestDto>();
        Log.Warning($"Test {test.Id}");
        foreach (var testTaskVersion in test.TestTaskVersions)
        {
            Log.Warning($"Task Id {testTaskVersion.TaskId} Task Version Id {testTaskVersion.TaskId}");
        }

        foreach (var testTaskVersion in test.TestTaskVersions)
        {
            versions.Add(await _versionService.GetAsync(testTaskVersion.TaskId, testTaskVersion.TaskVersionId));
        }

        result.TaskVersions = versions;

        return result;
    }

    public async Task<List<TestDto>> GetAllAsync(GetTestsRequestDto dto, string userId)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var tests = context.Tests
            .Include(t => t.TestTaskVersions)
            .AsQueryable();

        if (!string.IsNullOrEmpty(dto.Name))
        {
            tests = tests.Where(t => t.Name.Contains(dto.Name));
        }

        if (dto.IsAutoGenerated.HasValue)
        {
            if (dto.IsAutoGenerated.Value)
            {
                tests = tests.Where(t => t.IsAutoGenerated && t.CreatorId == userId);
            } else
            {
                tests = tests.Where(t => !t.IsAutoGenerated);
            }
        }

        var uploadedTests = await tests.ToListAsync();
        if (dto.TaskCount != null)
        {
            uploadedTests = uploadedTests.Where(t => t.TestTaskVersions.Count == dto.TaskCount).ToList();
        }
        foreach (var test in uploadedTests)
        {
            Log.Warning($"Test {test.Id}");
            foreach (var testTaskVersion in test.TestTaskVersions)
            {
                Log.Warning($"Task Id {testTaskVersion.TaskId} Task Version Id {testTaskVersion.TaskId}");
            }
        }

        var result = new List<TestDto>();
        foreach (var test in uploadedTests)
        {
            var mapped = _mapper.Map<TestDto>(test);
            result.Add(mapped);
            var versions = new List<VersionForTestDto>();
            foreach (var testTaskVersion in test.TestTaskVersions)
            {
                Log.Warning($"Getting {testTaskVersion.TaskId} taskId {testTaskVersion.TaskVersionId} task version");
                versions.Add(await _versionService.GetAsync(testTaskVersion.TaskId, testTaskVersion.TaskVersionId));
            }

            mapped.TaskVersions = versions;
        }

        if (dto.ThemeIds != null)
        {
            var uploadedVersions = await _versionService.GetAllAsync(new Contracts.Tasks.Dto.GetTaskVersionsRequestDto
            {
                ThemeIds = dto.ThemeIds
            });
            var uploadedVersionsIds = uploadedVersions.Select(v => v.Id).ToHashSet();
            result = result
                .Where(t => t.TaskVersions
                    .Any(v => uploadedVersionsIds
                        .Contains(v.Id)))
                .ToList();
        }

        return result;
    }
}
