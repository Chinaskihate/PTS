using System.ComponentModel.DataAnnotations;

namespace PTS.Contracts.PTSTestResults;
public class SubmitTaskDto
{
    [Required]
    public int TestResultId { get; set; }
    [Required]
    public int TaskVersionId { get; set; }
    [Required]
    public string Answer { get; set; }
}
