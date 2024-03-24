namespace PTS.Contracts.PTSTestResults;
public class TaskResultDto
{
    public int Id { get; set; }
    public int TaskVersionId { get; set; }
    public string Input { get; set; }
    public bool? IsCorrect { get; set; }
}
