using System.ComponentModel.DataAnnotations;

namespace PTS.Persistence.Models.Tests;
public class TestTaskVersion
{
    [Key]
    public int Id { get; set; }
    [Required]
    public Test Test { get; set; }
    [Required]
    public int TaskVersionId { get; set; }
    [Required]
    public int TaskId { get; set; }
}
