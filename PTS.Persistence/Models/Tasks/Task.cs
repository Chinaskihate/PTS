using PTS.Contracts.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PTS.Persistence.Models.Tasks;
public class Task
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    public bool IsEnabled { get; set; } = true;
    [Required]
    public TaskComplexity Complexity { get; set; }
    [Required]
    public int AvgTimeInMin { get; set; }
}
