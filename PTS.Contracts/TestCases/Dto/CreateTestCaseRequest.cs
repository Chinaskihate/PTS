using System.ComponentModel.DataAnnotations;

namespace PTS.Contracts.TestCases.Dto;
public class CreateTestCaseRequest
{
    public string? Input { get; set; }
    [Required]
    public string Output { get; set; } = string.Empty;
    public bool? IsCorrect { get; set; }
}
