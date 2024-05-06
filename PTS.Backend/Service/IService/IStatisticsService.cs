using PTS.Contracts.Statistics;

namespace PTS.Backend.Service.IService;
public interface IStatisticsService
{
    Task<List<TestResultStatisticsDto>> GetUserStats(string userId);
    Task<TaskStatisticsDto> GetTaskStats(int taskId);
    Task<TestStatisticsDto> GetTestStats(int testId);
}
