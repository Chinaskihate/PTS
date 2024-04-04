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
    [Required]
    public TaskType Type { get; set; }
    [Required]
    public List<Theme> Themes { get; set; } = [];
    public List<TaskVersion> Versions { get; set; } = [];
}
