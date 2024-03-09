using PTS.Contracts.Tasks.Dto;
using PTS.Contracts.TestCases.Dto;

namespace PTS.Backend.Service.IService;
public interface ITestCaseService
{
    Task<TaskDto> CreateAsync(int taskId, int versionId, CreateTestCaseRequest dto);
    Task<TaskDto> EditAsync(int taskId, int versionId, int caseId, EditTestCaseRequest dto);
    Task<TaskDto> DeleteAsync(int taskId, int versionId, int caseId);
}
