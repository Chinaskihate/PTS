using PTS.Contracts.Versions.Dto;

namespace PTS.Contracts.Tasks.Dto;
public class EditTaskRequest
{
    public bool IsEnabled { get; set; }
    public List<CreateVersionRequest>? NewVersions { get; set; }
    public List<EditVersionWithIdRequest>? EditedVersions { get; set; }
}
