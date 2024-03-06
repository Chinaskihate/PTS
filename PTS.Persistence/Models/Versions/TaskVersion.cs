using PTS.Persistence.Models.TestCases;
using System.ComponentModel.DataAnnotations;
using Task = PTS.Persistence.Models.Tasks.Task;

namespace PTS.Persistence.Models.Versions;
public class TaskVersion
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Description { get; set; } = string.Empty;
    public string? InputCondition { get; set; }
    public string? OutputCondition { get; set; }
    [Required]
    public int ProgrammingLanguage { get; set; }
    [Required]
    public Task Task { get; set; } = null!;
    public List<TestCase> TestCases { get; set; } = [];
}
