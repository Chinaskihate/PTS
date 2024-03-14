using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PTS.Backend.Exceptions.Common;
using PTS.Backend.Service.IService;
using PTS.Contracts.Test;
using PTS.Contracts.Tests.Dto;
using PTS.Contracts.Versions.Dto;
using PTS.Persistence.DbContexts;
using PTS.Persistence.Models.Tests;
using PTS.Persistence.Models.Tests.Versions;

namespace PTS.Backend.Service;
public class TestService(
    IDbContextFactory<TestDbContext> dbContextFactory,
    ITaskVersionProxyService versionService,
    IMapper mapper) : ITestService
{
    private readonly IDbContextFactory<TestDbContext> _dbContextFactory = dbContextFactory;
    private readonly ITaskVersionProxyService _versionService = versionService;
    private readonly IMapper _mapper = mapper;

    public async Task<TestDto> Create(CreateTestRequest dto)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var versions = new List<VersionForTestDto>();
        var test = new Test()
        {
            Name = dto.Name,
            Description = dto.Description,
            IsEnabled = dto.IsEnabled,
        };

        foreach (var item in dto.Versions)
        {
            versions.Add(await _versionService.GetAsync(item.TaskId, item.VersionId));
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

    public Task<TestDto> Get(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TestDto>> GetAllAsync()
    {
        using var context = _dbContextFactory.CreateDbContext();
        var tests = await context.Tests
            .Include(t => t.TestTaskVersions)
            .ToListAsync();

        var result = new List<TestDto>();
        foreach (var test in tests)
        {
            var mapped = _mapper.Map<TestDto>(test);
            result.Add(mapped);
            var versions = new List<VersionForTestDto>();
            foreach (var testTaskVersion in test.TestTaskVersions)
            {
                versions.Add(await _versionService.GetAsync(testTaskVersion.TaskId, testTaskVersion.TaskVersionId));
            }

            mapped.TaskVersions = versions;
        }

        return result;
    }
}
