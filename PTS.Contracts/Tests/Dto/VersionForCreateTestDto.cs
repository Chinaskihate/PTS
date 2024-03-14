using System.ComponentModel.DataAnnotations;

namespace PTS.Contracts.Tests.Dto;
public class VersionForCreateTestDto
{
    [Required]
    public int TaskId { get;set; }
    [Required]
    public int VersionId { get; set; }
}
