using PTS.Contracts.Statistics;

namespace PTS.Backend.Service.IService;
public interface IStatisticsService
{
    Task<List<TestStatisticsDto>> GetUserStats(string userId);
}
