using PTS.Contracts.PTSTestResults;
using PTS.Contracts.Tasks.Dto;
using PTS.Contracts.Versions.Dto;

namespace PTS.Backend.Service.IService;
public interface ITaskVersionProxyService
{
    Task<VersionForTestDto> GetAsync(int taskId, int versionId);
    Task<List<VersionForTestResultDto>> GetFullAsync(GetTaskVersionsForTestResultRequestDto dto);
    Task<List<VersionForTestDto>> GetAllAsync(GetTaskVersionsRequestDto dto);
}
