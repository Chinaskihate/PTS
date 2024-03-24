using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PTS.Backend.Exceptions.Common;
using PTS.Backend.Exceptions.TaskResult;
using PTS.Backend.Service.IService;
using PTS.Contracts.Constants;
using PTS.Contracts.PTSTestResults;
using PTS.Contracts.Tasks;
using PTS.Contracts.Tasks.Dto;
using PTS.Persistence.DbContexts;
using PTS.Persistence.Models.Results;
using System.Reflection.Metadata;

namespace PTS.Backend.Service;
public class TestExecutionService(
    IDbContextFactory<TestDbContext> dbContextFactory,
    ITaskVersionProxyService taskVersionService,
    ITestService testService,
    IMapper mapper) : ITestExecutionService
{
    private readonly IDbContextFactory<TestDbContext> _dbContextFactory = dbContextFactory;
    private readonly ITaskVersionProxyService _taskVersionService = taskVersionService;
    private readonly ITestService _testService = testService;
    private readonly IMapper _mapper = mapper;

    public async Task<TestResultDto[]> GetUserTestsAsync(string userId)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var tests = await context.TestResults
            .Include(t => t.Test)
            .Where(t => t.StudentId == userId)
            .ToListAsync();

        return _mapper.Map<TestResultDto[]>(tests);
    }

    public async Task<TestResultDto> StartTestAsync(StartTestDto dto, string userId)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var test = await context.Tests.FirstOrDefaultAsync(t => t.Id == dto.TestId)
            ?? throw new NotFoundException($"Test with {dto.TestId} id not found");
        var result = new TestResult()
        {
            Test = test,
            StudentId = userId,
        };
        context.TestResults.Add(result);
        await context.SaveChangesAsync();

        return _mapper.Map<TestResultDto>(result);
    }

    public async Task<TestResultDto> SubmitTaskAsync(SubmitTaskDto dto, string userId)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var testResult = await context.TestResults
            .Include(t => t.TaskResults)
            .Include(t => t.Test)
            .ThenInclude(t => t.TestTaskVersions)
            .FirstOrDefaultAsync(t => t.Id == dto.TestResultId && t.StudentId == userId)
            ?? throw new NotFoundException($"TestResult with {dto.TestResultId} id not found");

        var taskVersions = await _taskVersionService.GetFullAsync(new GetTaskVersionsForTestResultRequestDto
        {
            TaskVersionsIds = testResult.Test.TestTaskVersions.Select(v => v.TaskVersionId).ToArray()
        });
        var version = taskVersions.FirstOrDefault(v => v.Id == dto.TaskVersionId)
            ?? throw new NotFoundException($"Version with {dto.TaskVersionId} not found");

        var isCorrect = CheckAnswer(testResult, version, dto);
        var taskResult = new TaskResult
        {
            Input = dto.Answer,
            IsCorrect = isCorrect,
            TaskVersionId = dto.TaskVersionId,
            TestResult = testResult
        };
        context.TaskResults.Add(taskResult);
        await context.SaveChangesAsync();

        return _mapper.Map<TestResultDto>(testResult);
    }

    private bool CheckAnswer(TestResult testResult, VersionForTestResultDto version, SubmitTaskDto dto)
    {
        if (version.Type != TaskType.ExecutableCode
            && testResult.TaskResults.Any(r => r.TaskVersionId == dto.TaskVersionId))
        {
            throw new TaskAlreadySubmittedException("Task already submitted");
        }

        switch (version.Type)
        {
            case TaskType.SingleChoice:
                return CheckSingleChoiceTask(version, dto);
            case TaskType.MultipleChoice:
                return CheckMultipleChoiceTask(version, dto);
            case TaskType.StringAnswer:
                return CheckStringAnswerTask(version, dto);
            case TaskType.ExecutableCode:
                return CheckExecutableCodeTask(version, dto);
        }

        return false;
    }

    private bool CheckStringAnswerTask(VersionForTestResultDto version, SubmitTaskDto dto)
    {
        var testCase = version.TestCases.First();
        return testCase.Output == dto.Answer.Trim();
    }

    private bool CheckSingleChoiceTask(VersionForTestResultDto version, SubmitTaskDto dto)
    {
        var testCase = version.TestCases.First(c => c.IsCorrect == true);
        return testCase.Output == dto.Answer.Trim();
    }

    private bool CheckMultipleChoiceTask(VersionForTestResultDto version, SubmitTaskDto dto)
    {
        var correctAnswers = version.TestCases.Where(c => c.IsCorrect == true).Select(c => c.Output).ToList();
        var studentChoices = dto.Answer.Trim().Split(Constants.MultipleChoiceDelimiter);
        foreach (var choice in studentChoices)
        {
            if (!correctAnswers.Contains(choice))
                return false;
        }

        return true;
    }

    private bool CheckExecutableCodeTask(VersionForTestResultDto version, SubmitTaskDto dto)
    {
        var testCase = version.TestCases.First(c => c.IsCorrect == true);
        return testCase.Output == dto.Answer.Trim();
    }
}
