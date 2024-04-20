using System.ComponentModel.DataAnnotations;

namespace PTS.Contracts.PTSTestResults;
public class SubmitTaskDto
{
    [Required]
    public int TestResultId { get; set; }
    public int? TaskVersionId { get; set; }
    public string? Answer { get; set; }
    public bool? ForceFinish { get; set; }
}
