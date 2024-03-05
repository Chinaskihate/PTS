using System.ComponentModel.DataAnnotations;

namespace PTS.Contracts.Tasks.Dto;
public class CreateTaskRequestDto
{
    [Required]
    public int[] ThemeIds { get; set; } = [];
    [Required]
    public ProgrammingLanguage ProgrammingLanguage { get; set; }
    [Required]
    public bool IsEnabled { get; set; }
    [Required]
    public TaskType Type { get; set; }
    [Required]
    public TaskComplexity Complexity { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    [Required]
    public int AvgTimeInMin { get; set; }
    public string? InputCondition { get; set; }
    public string? OutputCondition { get; set; }
}
