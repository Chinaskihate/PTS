using PTS.Contracts.Tasks;
using PTS.Contracts.Theme.Dto;
using System.ComponentModel.DataAnnotations;

namespace PTS.Contracts.Versions.Dto;
public class VersionForTestDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int TaskId { get; set; }
    [Required]
    public ProgrammingLanguage ProgrammingLanguage { get; set; }
    [Required]
    public TaskType Type { get; set; }
    [Required]
    public string Description { get; set; } = string.Empty;
    public string? InputCondition { get; set; } = null;
    public string? OutputCondition { get; set; } = null;
    [Required]
    public List<ThemeForTestDto> Themes { get; set; }
}