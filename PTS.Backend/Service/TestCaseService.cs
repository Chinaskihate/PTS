using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PTS.Backend.Service.IService;
using PTS.Contracts.Tasks.Dto;
using PTS.Contracts.TestCases.Dto;
using PTS.Persistence.DbContexts;
using PTS.Persistence.Models.TestCases;

namespace PTS.Backend.Service;
public class TestCaseService(
    IDbContextFactory<TaskDbContext> dbContextFactory,
    IMapper mapper) : ITestCaseService
{
    private readonly IDbContextFactory<TaskDbContext> _dbContextFactory = dbContextFactory;
    private readonly IMapper _mapper = mapper;

    public async Task<TaskDto> CreateAsync(int taskId, int versionId, CreateTestCaseRequest dto)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var task = await context.Tasks
            .Include(t => t.Versions)
            .ThenInclude(v => v.TestCases)
            .FirstOrDefaultAsync(t => t.Id == taskId);
        var version = task.Versions.FirstOrDefault(v => v.Id == versionId);
        var testCase = new TestCase
        {
            Input = dto.Input,
            Output = dto.Output,
            IsCorrect = dto.IsCorrect,
            Version = version
        };

        await context.SaveChangesAsync();

        return _mapper.Map<TaskDto>(task);
    }
    public Task<TaskDto> DeleteAsync(int taskId, int versionId, int caseId)
    {
        throw new NotImplementedException();
    }

    public Task<TaskDto> EditAsync(int taskId, int versionId, int caseId, EditTestCaseRequest dto)
    {
        throw new NotImplementedException();
    }
}
