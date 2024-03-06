using PTS.Contracts.Tasks.Dto;

namespace PTS.Backend.Service.IService;
public interface ITaskService
{
    Task<TaskDto> CreateAsync(CreateTaskRequestDto dto);
    Task<TaskDto> EditAsync(int id, EditTaskRequest dto);
    Task<TaskDto> GetAsync(int id);
}
