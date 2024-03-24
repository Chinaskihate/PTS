using PTS.Contracts.PTSTestResults;
using PTS.Contracts.Tasks.Dto;
using PTS.Contracts.Versions.Dto;

namespace PTS.Backend.Service.IService;
public interface ITaskVersionService
{
    Task<TaskDto> CreateAsync(int taskId, CreateVersionRequest dto);
    Task<TaskDto> EditAsync(int taskId, int versionId, EditVersionRequest dto);
    Task<TaskDto> EditAsync(int taskId, EditVersionWithIdRequest dto);
    Task<VersionForTestDto[]> GetAllAsync(GetTaskVersionsRequestDto dto);
    Task<VersionForTestResultDto[]> GetAsync(GetTaskVersionsForTestResultRequestDto dto);
}
