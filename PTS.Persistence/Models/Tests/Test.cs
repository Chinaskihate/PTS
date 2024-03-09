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
    public List<TestTaskVersion> TestTaskVersions { get; set; } = [];
}
