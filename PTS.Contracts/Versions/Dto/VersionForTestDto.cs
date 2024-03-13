using PTS.Contracts.Tasks;
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
    public string Description { get; set; } = string.Empty;
    public string? InputCondition { get; set; } = null;
    public string? OutputCondition { get; set; } = null;
}