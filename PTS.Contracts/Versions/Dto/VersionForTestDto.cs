using PTS.Contracts.Tasks;
using PTS.Contracts.TestCases.Dto;
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
    public string? Name { get; set; }
    public TaskComplexity? Complexity { get; set; }
    public int? AvgTimeInMin { get; set; }
    public string? Description { get; set; }
    public string? InputCondition { get; set; } = null;
    public string? OutputCondition { get; set; } = null;
    public List<ThemeForTestDto> Themes { get; set; }
    public CodeTemplateDto? CodeTemplate { get; set; }
    public List<TestCaseForStudentDto>? TestCases { get; set; }
}