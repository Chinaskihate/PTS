using System.ComponentModel.DataAnnotations;

namespace PTS.Contracts.Versions.Dto;
public class EditVersionRequest
{
    [Required]
    public string Description { get; set; } = string.Empty;
}
