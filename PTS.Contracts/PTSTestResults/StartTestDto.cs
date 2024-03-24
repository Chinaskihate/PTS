using System.ComponentModel.DataAnnotations;

namespace PTS.Contracts.PTSTestResults;
public class StartTestDto
{
    [Required]
    public int TestId { get; set; }
}
