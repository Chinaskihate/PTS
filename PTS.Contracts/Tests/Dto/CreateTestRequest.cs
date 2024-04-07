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
    public int AllowedExecutionTimeInSec { get; set; }
    [Required]
    public List<VersionForCreateTestDto> Versions { get; set; } 
}
