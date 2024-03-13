using PTS.Contracts.Versions.Dto;
using System.ComponentModel.DataAnnotations;

namespace PTS.Contracts.Tests.Dto;
public class CreateTestRequest
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public bool IsEnabled { get; set; }
    [Required]
    public List<VersionForTestDto> Versions { get; set; } 
}
