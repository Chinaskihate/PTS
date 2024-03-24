using PTS.Contracts.Test;

namespace PTS.Contracts.PTSTestResults;
public class TestResultDto
{
    public int Id { get; set; }
    public TestDto Test { get; set; }
    public string StudentId { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? SubmissionTime { get; set; }
    public List<int>? CompletedTaskVersionIds { get; set; }
    public List<int>? UncompletedTaskVersionIds { get; set; }
}
