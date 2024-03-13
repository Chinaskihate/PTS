using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PTS.Backend.Exceptions.Common;
using PTS.Backend.Service.IService;
using PTS.Contracts.Tasks.Dto;
using PTS.Contracts.Versions.Dto;
using PTS.Persistence.DbContexts;
using PTS.Persistence.Models.Versions;
using System;

namespace PTS.Backend.Service;
public class TaskVersionService(
    IDbContextFactory<TaskDbContext> dbContextFactory,
    ITestCaseService testCaseService,
    IMapper mapper) : ITaskVersionService
{
    private readonly IDbContextFactory<TaskDbContext> _dbContextFactory = dbContextFactory;
    private readonly ITestCaseService _testCaseService = testCaseService;
    private readonly IMapper _mapper = mapper;

    public async Task<TaskDto> CreateAsync(int taskId, CreateVersionRequest dto)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var task = context.Tasks
            .Include(t => t.Themes)
            .Include(t => t.Versions)
            .ThenInclude(v => v.TestCases)
            .FirstOrDefault(t => t.Id == taskId) ?? throw new NotFoundException($"Task {taskId} not found");
        var version = new TaskVersion()
        {
            Description = dto.Description,
            InputCondition = dto.InputCondition,
            OutputCondition = dto.OutputCondition,
            ProgrammingLanguage = (int)dto.ProgrammingLanguage,
            Task = task
        };

        context.TaskVersions.Add(version);

        await context.SaveChangesAsync();

        TaskDto result = _mapper.Map<TaskDto>(task);
        foreach (var testCase in dto.TestCases)
        {
            result = await _testCaseService.CreateAsync(taskId, version.Id, testCase);
        }

        return result;
    }

    public async Task<TaskDto> EditAsync(int taskId, int versionId, EditVersionRequest dto)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var task = context.Tasks
            .Include(t => t.Themes)
            .Include(t => t.Versions)
            .ThenInclude(v => v.TestCases)
            .FirstOrDefault(t => t.Id == taskId) ?? throw new NotFoundException($"Task {taskId} not found");
        var version = task.Versions
            .FirstOrDefault(v => v.Id == versionId) ?? throw new NotFoundException($"Version {versionId} not found");

        version.Description = dto.Description;
        version.InputCondition = dto.InputCondition;
        version.OutputCondition = dto.OutputCondition;

        await context.SaveChangesAsync();

        TaskDto result = _mapper.Map<TaskDto>(task);
        foreach (var testCase in dto.NewTestCases)
        {
            result = await _testCaseService.CreateAsync(taskId, version.Id, testCase);
        }

        foreach (var testCase in dto.EditedTestCases)
        {
            result = await _testCaseService.EditAsync(taskId, version.Id, testCase);
        }

        return result;
    }

    public async Task<TaskDto> EditAsync(int taskId, EditVersionWithIdRequest dto)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var task = context.Tasks
            .Include(t => t.Themes)
            .Include(t => t.Versions)
            .ThenInclude(v => v.TestCases)
            .FirstOrDefault(t => t.Id == taskId) ?? throw new NotFoundException($"Task {taskId} not found");
        var version = task.Versions
            .FirstOrDefault(v => v.Id == dto.Id) ?? throw new NotFoundException($"Version {dto.Id} not found");

        version.Description = dto.Description;
        version.InputCondition = dto.InputCondition;
        version.OutputCondition = dto.OutputCondition;

        await context.SaveChangesAsync();

        TaskDto result = _mapper.Map<TaskDto>(task);
        foreach (var testCase in dto.NewTestCases)
        {
            result = await _testCaseService.CreateAsync(taskId, version.Id, testCase);
        }

        foreach (var testCase in dto.EditedTestCases)
        {
            result = await _testCaseService.EditAsync(taskId, version.Id, testCase);
        }

        return result;
    }
}
