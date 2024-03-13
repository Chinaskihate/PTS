using PTS.Contracts.Versions.Dto;
using System.ComponentModel.DataAnnotations;

namespace PTS.Contracts.Tasks.Dto;
public class CreateTaskRequestDto
{
    [Required]
    public int[] ThemeIds { get; set; } = [];
    [Required]
    public bool IsEnabled { get; set; }
    [Required]
    public TaskType Type { get; set; }
    [Required]
    public TaskComplexity Complexity { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public int AvgTimeInMin { get; set; }
    [Required]
    public List<CreateVersionRequest> Versions { get; set; }
}
