using System.ComponentModel.DataAnnotations;

namespace PTS.Persistence.Models.Tasks;
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
