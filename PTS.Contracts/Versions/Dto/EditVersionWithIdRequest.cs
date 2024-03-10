using PTS.Contracts.TestCases.Dto;
using System.ComponentModel.DataAnnotations;

namespace PTS.Contracts.Versions.Dto;
public class EditVersionWithIdRequest
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Description { get; set; } = string.Empty;
    public string? InputCondition { get; set; }
    public string? OutputCondition { get; set; }
    public List<CreateTestCaseRequest> NewTestCases { get; set; }
    public List<EditTestCaseWithIdRequest> EditedTestCases { get; set; }
}
