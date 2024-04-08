using System.ComponentModel.DataAnnotations;

namespace PTS.Persistence.Models.Tests;
public class Test
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    [Required]
    public bool IsEnabled { get; set; }
    [Required]
    public string CreatorId { get; set; }
    [Required]
    public int AllowedExecutionTimeInSec { get; set; }
    public List<TestTaskVersion> TestTaskVersions { get; set; } = [];
    public List<TestResult> TestResults { get; set; }
}
