﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PTS.Backend.Exceptions.Common;
using PTS.Backend.Service.IService;
using PTS.Contracts.PTSTestResults;
using PTS.Contracts.Statistics;
using PTS.Contracts.Tasks;
using PTS.Contracts.Tasks.Dto;
using PTS.Contracts.Tests.Dto;
using PTS.Persistence.DbContexts;
using PTS.Persistence.Models.Tests;
using Z.Expressions;

namespace PTS.Backend.Service;
public class StatisticsService(
    IDbContextFactory<TestDbContext> dbContextFactory,
    ITestService testService,
    ITaskVersionProxyService taskVersionProxyService,
    IMapper mapper) : IStatisticsService
{
    private readonly IDbContextFactory<TestDbContext> _dbContextFactory = dbContextFactory;
    private readonly ITestService _testService = testService;
    private readonly ITaskVersionProxyService _taskVersionProxyService = taskVersionProxyService;
    private readonly IMapper _mapper = mapper;

    public async Task<TaskStatisticsDto> GetTaskStats(int taskId)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var ttvs = await context.TestTaskVersions.Where(t => t.TaskId == taskId)
            .ToListAsync();
        if (ttvs.Count == 0)
        {
            throw new NotFoundException($"Task {taskId} does not contain in any test");
        }

        var result = new TaskStatisticsDto()
        {
            TaskId = taskId
        };
        foreach (var ttv in ttvs)
        {
            result.SuccessfulSubmissionCount += ttv.SuccessfulSubmissionCount;
            result.TotalSubmissionCount += ttv.TotalSubmissionCount;
        }

        return result;
    }

    public async Task<TestStatisticsDto> GetTestStats(int testId)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var test = await context.Tests
            .Include(t => t.TestTaskVersions)
            .FirstOrDefaultAsync(t => t.Id == testId)
            ?? throw new NotFoundException($"Test {testId} does not contain in any test");

        var result = new TestStatisticsDto()
        {
            TestId = testId
        };
        foreach (var ttv in test.TestTaskVersions)
        {
            result.SuccessfulTaskSubmissionCount += ttv.SuccessfulSubmissionCount;
            result.TotalTaskSubmissionCount += ttv.TotalSubmissionCount;
        }

        result.TotalSubmissionCount = test.TotalSubmissionCount;

        return result;
    }

    public async Task<List<TestResultStatisticsDto>> GetUserStats(string userId)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var testResults = await context.TestResults
            .Include(t => t.TaskResults)
            .Include(t => t.Test)
            .ThenInclude(t => t.TestTaskVersions)
            .Where(t => t.StudentId == userId && t.SubmissionTime != null)
            .ToListAsync();

        var mappedTestsResults = new List<TestResultStatisticsDto>();
        foreach (var testResult in testResults)
        {
            var test = await _testService.Get(testResult.Test.Id);
            var correctAnswers = await _taskVersionProxyService.GetFullAsync(new GetTaskVersionsForTestResultRequestDto
            {
                TaskVersionsIds = test.TaskVersions.Select(v => v.Id).ToArray(),
            });
            var mappedTestResults = _mapper.Map<TestResultStatisticsDto>(testResult);
            mappedTestResults.Test = test;
            mappedTestResults.TaskResultStatuses = GetTaskStatuses(testResult, correctAnswers);
            mappedTestsResults.Add(mappedTestResults);
        }

        return mappedTestsResults;
    }

    private List<TaskResultStatus> GetTaskStatuses(TestResult testResult, List<VersionForTestResultDto> correctAnswers)
    {
        var result = new List<TaskResultStatus>();
        foreach (var taskResult in testResult.TaskResults)
        {
            var version = correctAnswers.First(a => a.Id == taskResult.TaskVersionId);
            var status = new TaskResultStatus
            {
                TaskVersionId = taskResult.TaskVersionId,
                StudentAnswer = taskResult.Input,
                IsCorrect = taskResult.IsCorrect.Value
            };
            if (version.Type != TaskType.ExecutableCode)
            {
                status.CorrectAnswers = version.TestCases
                        .Where(c => c.IsCorrect.Value)
                        .Select(c => c.Output)
                        .ToList();
            }
            else
            {
                var testCases = version.TestCases.Where(c => c.IsCorrect == true).ToList();
                var codeTaskInfos = new List<CodeTaskInfo>();
                foreach (var testCase in testCases)
                {
                    var codeTaskInfo = new CodeTaskInfo
                    {
                        TestCaseId = testCase.Id,
                        Input = testCase.Input
                    };
                    var data = new InputData
                    {
                        Values = testCase.Input.Split(' ').ToList()
                    };
                    var output = Eval.Execute<string>(taskResult.Input, data);
                    codeTaskInfo.ActualOutput = output;
                    codeTaskInfo.ExpectedOutput = testCase.Output;
                    codeTaskInfo.IsCorrect = output == testCase.Output;
                    codeTaskInfos.Add(codeTaskInfo);
                }
                status.CodeTaskInfos = codeTaskInfos;
            }
            result.Add(status);
        }
        return result;
    }
}
