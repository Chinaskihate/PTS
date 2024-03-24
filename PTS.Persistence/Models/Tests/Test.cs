using PTS.Persistence.Models.Results;
using PTS.Persistence.Models.Tests.Versions;
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
    public List<TestTaskVersion> TestTaskVersions { get; set; } = [];
    public List<TestResult> TestResults { get; set; }
}
