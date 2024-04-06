using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTS.Backend.Service.IService;
using PTS.Contracts.Common;
using PTS.Contracts.Tasks.Dto;
using PTS.Contracts.Users;

namespace PTS.ThemeAPI.Controllers;
[ApiController]
[Route("api/task")]
[Authorize]
public class TaskController(ITaskService taskService) : ControllerBase
{
    private readonly ResponseDto _response = new();
    private readonly ITaskService _taskService = taskService;

    [HttpPost]
    [Authorize(Roles = UserRoles.TaskManagerRoles)]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequestDto dto)
    {
        _response.Result = await _taskService.CreateAsync(dto);
        return Ok(_response);
    }

    [HttpPost("{id:int}")]
    [Authorize(Roles = UserRoles.TaskManagerRoles)]
    public async Task<IActionResult> Edit(int id, [FromBody] EditTaskRequest dto)
    {
        _response.Result = await _taskService.EditAsync(id, dto);
        return Ok(_response);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = UserRoles.TaskManagerRoles)]
    public async Task<IActionResult> Get(int id)
    {
        _response.Result = await _taskService.GetAsync(id);
        return Ok(_response);
    }

    [HttpGet()]
    [Authorize(Roles = UserRoles.TaskManagerRoles)]
    public async Task<IActionResult> GetAllAsync()
    {
        _response.Result = await _taskService.GetAllAsync();
        return Ok(_response);
    }
}
