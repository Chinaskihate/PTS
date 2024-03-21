using PTS.Contracts.Versions.Dto;

namespace PTS.Contracts.Tasks.Dto;
public class EditTaskRequest
{
    //[Required]
    //public int[] ThemeIds { get; set; } = [];
    public bool IsEnabled { get; set; }
    public List<CreateVersionRequest>? NewVersions { get; set; }
    public List<EditVersionWithIdRequest>? EditedVersions { get; set; }
}
