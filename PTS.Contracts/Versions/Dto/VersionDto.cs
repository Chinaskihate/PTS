using PTS.Contracts.Tasks;

namespace PTS.Contracts.Versions.Dto;
public class VersionDto
{
    public int Id { get; set; }
    public ProgrammingLanguage ProgrammingLanguage { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? InputCondition { get; set; } = null;
    public string? OutputCondition { get; set; } = null;
}