namespace PTS.Contracts.Statistics;
public class TaskStatisticsDto
{
    public int TaskId { get; set; }
    public int SuccessfulSubmissionCount { get; set; }
    public int TotalSubmissionCount { get; set; }
}
