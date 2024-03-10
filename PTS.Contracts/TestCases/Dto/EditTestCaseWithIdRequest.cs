using System.ComponentModel.DataAnnotations;

namespace PTS.Contracts.TestCases.Dto;
public class EditTestCaseWithIdRequest
{
    [Required]
    public int Id { get; set; }
    public string? Input { get; set; }
    [Required]
    public string Output { get; set; } = string.Empty;
    public bool? IsCorrect { get; set; }
}
