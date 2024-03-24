using PTS.Backend.Service.IService;
using PTS.Contracts.Tasks.Dto;
using PTS.Contracts.Tests.Dto;
using PTS.Contracts.Versions.Dto;

namespace PTS.Backend.Service;

public class TestGeneratorService : ITestGeneratorService
{
    public async Task<long> GenerateTest(GenerateTestRequest dto)
    {
        
        return await Task.FromResult(0);
    }
    
    private static List<VersionDto> GenerateTaskList(IEnumerable<TaskDto> allTasks, GenerateTestRequest request)
    {
        var filteredTasks = allTasks
            .Where(task => task.IsEnabled
                           && request.ProgrammingLanguage.Any(lang => task.Versions.Any(v => v.ProgrammingLanguage.ToString().Equals(lang, StringComparison.OrdinalIgnoreCase)))
                           && request.ThemeIds.Intersect(task.ThemeIds).Any()
                           && request.Difficult.Contains(task.Difficult))
            .ToList();

        var tasksCount = filteredTasks.Count;
        var testTime = request.Time ?? long.MaxValue;

        var selectedTasks = Filter(filteredTasks, testTime, tasksCount);

        return selectedTasks.SelectMany(task => task.Versions.Where(v => request.ProgrammingLanguage.Contains(v.ProgrammingLanguage.ToString()))).ToList();
    }

    private static IEnumerable<TaskDto> Filter(IReadOnlyList<TaskDto> tasks, long testTime, int taskCount)
    {
        var dp = new long[taskCount + 1, testTime + 1];

        for (var index = 0; index <= taskCount; index++)
        {
            for (var time = 0; time <= testTime; time++)
            {
                if (index == 0 || time == 0)
                    dp[index, time] = 0;
                else if (tasks[index - 1].Time <= time)
                    dp[index, time] = Math.Max(tasks[index - 1].Difficult + dp[index - 1, time - tasks[index - 1].Time], dp[index - 1, time]);
                else
                    dp[index, time] = dp[index - 1, time];
            }
        }

        var result = dp[taskCount, testTime];
        var selectedTasks = new List<TaskDto>();

        for (var index = taskCount; index > 0 && result > 0; index--)
        {
            if (result == dp[index - 1, testTime])
                continue;
            selectedTasks.Add(tasks[index - 1]);

            result -= tasks[index - 1].Difficult;
            testTime -= tasks[index - 1].Time;
        }

        return selectedTasks;
    }
}