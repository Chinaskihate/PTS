using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PTS.Backend.Exceptions.Common;
using PTS.Backend.Exceptions.TaskResult;
using PTS.Backend.Service.IService;
using PTS.Contracts.Constants;
using PTS.Contracts.PTSTestResults;
using PTS.Contracts.Tasks;
using PTS.Contracts.Tasks.Dto;
using PTS.Contracts.Tests.Dto;
using PTS.Persistence.DbContexts;
using PTS.Persistence.Models.Tests;
using Z.Expressions;

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

    public async Task<TestResultDto> GetTestStatusAsync(int testResultId, string userId)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var testResult = await context.TestResults
            .Include(t => t.TaskResults)
            .Include(t => t.Test)
            .ThenInclude(t => t.TestTaskVersions)
            .FirstOrDefaultAsync(t => t.StudentId == userId)
            ?? throw new NotFoundException($"TestResult with {testResultId} id not found");

        var mappedTestResult = _mapper.Map<TestResultDto>(testResult);
        (mappedTestResult.CompletedTaskVersionIds, mappedTestResult.UncompletedTaskVersionIds) = GetTestStatus(testResult);

        return mappedTestResult;
    }

    public async Task<List<TestResultDto>> GetUserTestsAsync(string userId)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var testResults = await context.TestResults
            .Include(t => t.TaskResults)
            .Include(t => t.Test)
            .ThenInclude(t => t.TestTaskVersions)
            .Where(t => t.StudentId == userId)
            .ToListAsync();

        var mappedTestResults = new List<TestResultDto>();
        foreach (var testResult in testResults)
        {
            var mappedTestResult = _mapper.Map<TestResultDto>(testResult);
            (mappedTestResult.CompletedTaskVersionIds, mappedTestResult.UncompletedTaskVersionIds) = GetTestStatus(testResult);
            mappedTestResults.Add(mappedTestResult);
        }

        return mappedTestResults;
    }

    public async Task<TestResultDto> StartTestAsync(StartTestDto dto, string userId)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var test = await context.Tests
            .Include(t => t.TestTaskVersions)
            .FirstOrDefaultAsync(t => t.Id == dto.TestId)
            ?? throw new NotFoundException($"Test with {dto.TestId} id not found");
        var result = new TestResult()
        {
            Test = test,
            StudentId = userId,
        };
        context.TestResults.Add(result);
        await context.SaveChangesAsync();

        var mapped = _mapper.Map<TestResultDto>(result);
        mapped.UncompletedTaskVersionIds = test.TestTaskVersions.Select(v => v.TaskVersionId).ToList();
        return mapped;
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
        if (dto.ForceFinish == true)
        {
            (var _, var taskVersionIdsToComplete) = GetTestStatus(testResult);
            foreach (var taskVersionId in taskVersionIdsToComplete)
            {
                context.TaskResults.Add(new TaskResult
                {
                    Input = Constants.AnswerTemplateOnForceFinish,
                    IsCorrect = false,
                    TaskVersionId = taskVersionId,
                    TestResult = testResult
                });
            }
        }
        else
        {
            var version = taskVersions.FirstOrDefault(v => v.Id == dto.TaskVersionId)
            ?? throw new NotFoundException($"Version with {dto.TaskVersionId} not found");

            var isCorrect = CheckAnswer(version, dto);
            var taskResult = new TaskResult
            {
                Input = dto.Answer,
                IsCorrect = isCorrect,
                TaskVersionId = dto.TaskVersionId.Value,
                TestResult = testResult
            };
            context.TaskResults.Add(taskResult);
        }


        (var completedTaskVersionIds, var uncompletedTaskVersionIds) = GetTestStatus(testResult);
        if (uncompletedTaskVersionIds.Count == 0)
        {
            testResult.SubmissionTime = DateTime.UtcNow;
        }

        await context.SaveChangesAsync();
        var mappedTest = _mapper.Map<TestResultDto>(testResult);
        mappedTest.CompletedTaskVersionIds = completedTaskVersionIds;
        mappedTest.UncompletedTaskVersionIds = uncompletedTaskVersionIds;

        return mappedTest;
    }

    private bool CheckAnswer(VersionForTestResultDto version, SubmitTaskDto dto)
    {
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
        try
        {
            var testCases = version.TestCases.Where(c => c.IsCorrect == true).ToList();
            foreach (var testCase in testCases)
            {
                var data = new InputData
                {
                    Values = testCase.Input.Split(' ').ToList()
                };

                var output = Eval.Execute<string>(dto.Answer, data);
                if (output != testCase.Output)
                {
                    return false;
                }
            }

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private (List<int> CompletedTaskVersionIds, List<int> UncompletedTaskVersionIds) GetTestStatus(TestResult testResult)
    {
        var fullTaskVersionIds = testResult.Test.TestTaskVersions
            .Select(v => v.TaskVersionId)
            .Distinct()
            .ToList();
        var completedTaskVersionIds = testResult.TaskResults
            ?.Select(r => r.TaskVersionId)
            .Distinct()
            .ToList() ?? new List<int>();
        return (completedTaskVersionIds, fullTaskVersionIds.Except(completedTaskVersionIds).ToList());
    }
}
