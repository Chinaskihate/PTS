using System.ComponentModel.DataAnnotations;

namespace PTS.Persistence.Models.Tests;

public class TaskResult
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int TaskVersionId { get; set; }
    [Required]
    public string Input { get; set; }
    public bool? IsCorrect { get; set; }
    [Required]
    public TestResult TestResult { get; set; }
}
