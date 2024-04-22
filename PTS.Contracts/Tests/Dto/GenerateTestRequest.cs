using PTS.Contracts.Tasks;

namespace PTS.Contracts.Tests.Dto;

public class GenerateTestRequest
{
    public int? Time { get; set; }
    public int[] ThemeIds { get; set; } = [];
    public TaskComplexity Complexity { get; set; }
    public int TaskCount { get; set; }
    public int? ApproximateAllowedExecutionTimeInSec { get; set; }
}