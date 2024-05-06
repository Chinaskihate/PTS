using PTS.Contracts.Test;

namespace PTS.Contracts.Statistics;
public class TestResultStatisticsDto
{
    public int TestResultId { get; set; }
    public TestDto Test { get; set; }
    public string StudentId { get; set; }
    public DateTime SubmissionTime { get; set; }
    public List<TaskResultStatus> TaskResultStatuses { get; set; }
}

public class TaskResultStatus
{
    public int TaskVersionId { get; set; }
    public bool IsCorrect { get; set; }
    public string StudentAnswer { get; set; }
    public List<string>? CorrectAnswers { get; set; }
    public List<CodeTaskInfo>? CodeTaskInfos { get; set; }
}

public class CodeTaskInfo
{
    public int TestCaseId { get; set; }
    public string Input { get; set; }
    public string ExpectedOutput { get; set; }
    public string ActualOutput { get; set; }
    public bool IsCorrect { get; set; }
}