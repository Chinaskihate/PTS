using System.ComponentModel.DataAnnotations;

namespace PTS.Contracts.TestCases;
public class CreateTestCase
{
    public string? Input { get; set; }
    [Required]
    public string Output { get; set; } = string.Empty;
    public string? Variants { get; set; }
}
