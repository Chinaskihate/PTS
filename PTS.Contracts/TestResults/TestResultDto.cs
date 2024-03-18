using PTS.Contracts.Test;

namespace PTS.Contracts.TestResults;
public class TestResultDto
{
    public int Id { get; set; }
    public TestDto Test { get; set; }
    public string StudentId { get; set; }
}
