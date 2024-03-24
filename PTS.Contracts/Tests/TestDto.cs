using PTS.Contracts.Versions.Dto;

namespace PTS.Contracts.Test;
public class TestDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsEnabled { get; set; }
    public List<VersionForTestDto> TaskVersions { get; set; }
}
