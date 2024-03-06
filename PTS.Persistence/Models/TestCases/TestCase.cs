using PTS.Persistence.Models.Versions;
using System.ComponentModel.DataAnnotations;

namespace PTS.Persistence.Models.TestCases;
public class TestCase
{
    [Key]
    public int Id { get; set; }
    public string? Input { get; set; }
    public string Output { get; set; } = string.Empty;
    public bool? IsCorrect { get; set; }
    [Required]
    public TaskVersion Version { get; set; } = null!;
}
