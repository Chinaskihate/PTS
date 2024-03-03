using PTS.Contracts.Tasks.Dto;

namespace PTS.Backend.Service.IService;
public interface ITaskService
{
    Task<TaskDto> CreateAsync(CreateTaskRequestDto dto);
}
