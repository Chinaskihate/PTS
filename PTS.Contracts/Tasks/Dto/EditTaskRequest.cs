using PTS.Contracts.Versions.Dto;
using System.ComponentModel.DataAnnotations;

namespace PTS.Contracts.Tasks.Dto;
public class EditTaskRequest
{
    [Required]
    public int[] ThemeIds { get; set; } = [];
    public bool IsEnabled { get; set; }
    [Required]
    public List<CreateVersionRequest> NewVersions { get; set; }
    [Required]
    public List<EditVersionWithIdRequest> EditedVersions { get; set; }
}
