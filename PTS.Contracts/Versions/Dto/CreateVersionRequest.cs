using PTS.Contracts.Tasks;

namespace PTS.Contracts.Versions.Dto;
public class CreateVersionRequest
{
    public ProgrammingLanguage ProgrammingLanguage { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? InputCondition { get; set; }
    public string? OutputCondition { get; set;}
}
