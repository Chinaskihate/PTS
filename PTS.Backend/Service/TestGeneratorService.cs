﻿using PTS.Backend.Service.IService;
using PTS.Contracts.Tasks.Dto;
using PTS.Contracts.Tests.Dto;
using PTS.Contracts.Versions.Dto;

namespace PTS.Backend.Service;

public class TestGeneratorService(ITaskVersionProxyService versionService, ITestProxyService testService)
    : ITestGeneratorService
{
    private readonly ITaskVersionProxyService _versionService = versionService;
    private readonly ITestProxyService _testService = testService;

    public async Task<long> GenerateTest(GenerateTestRequest dto)
    {
        var versions = await GetTasks(dto);
        if (versions.Count == 0)
        {
            throw new ArgumentException("Cannot generate test");
        }

        var test = await _testService.Create(new CreateTestRequest
        {
            Name = "Generated test",
            Description = "Generate test",
            IsEnabled = true,
            IsAutoGenerated = true,
            Versions = versions.Select(it => new VersionForCreateTestDto
            {
                TaskId = it.TaskId,
                VersionId = it.Id
            }).ToList()
        });

        return await Task.FromResult(test.Id);
    }

    private async Task<List<VersionForTestDto>> GetTasks(GenerateTestRequest filter)
    {
        var versions = await _versionService.GetAllAsync(new GetTaskVersionsRequestDto
        {
            ThemeIds = filter.ThemeIds,
        });

        var resultVersions = GenerateTaskList(versions, filter);

        return resultVersions;
    }

    private static List<VersionForTestDto> GenerateTaskList(List<VersionForTestDto> filteredTasks,
        GenerateTestRequest request)
    {
        var tasksCount = filteredTasks.Count;
        var testTime = request.Time ?? int.MaxValue;
        return filteredTasks.Take(request.TaskCount).ToList();
    }

    private static IEnumerable<VersionForTestDto> Filter(IReadOnlyList<VersionForTestDto> tasks, int testTime,
        int taskCount)
    {
        var dp = new int[taskCount + 1, testTime + 1];

        for (var index = 0; index <= taskCount; index++)
        {
            for (var time = 0; time <= testTime; time++)
            {
                if (index == 0 || time == 0)
                    dp[index, time] = 0;
                else if (tasks[index - 1].AvgTimeInMin <= time)
                    dp[index, time] =
                        Math.Max(
                            (int)tasks[index - 1].Complexity! +
                            dp[index - 1, time - (tasks[index - 1].AvgTimeInMin ?? 0)], dp[index - 1, time]);
                else
                    dp[index, time] = dp[index - 1, time];
            }
        }

        var result = dp[taskCount, testTime];
        var selectedTasks = new List<VersionForTestDto>();

        for (var index = taskCount; index > 0 && result > 0; index--)
        {
            if (result == dp[index - 1, testTime])
                continue;
            selectedTasks.Add(tasks[index - 1]);

            result -= (int)tasks[index - 1].Complexity!;
            testTime -= tasks[index - 1].AvgTimeInMin ?? 0;
        }

        return selectedTasks;
    }
}