using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTS.Backend.Service.IService;
using PTS.Contracts.Common;
using PTS.Contracts.Tasks.Dto;
using PTS.Contracts.Users;
using PTS.Contracts.Versions.Dto;

namespace PTS.TaskAPI.Controllers;

[ApiController]
[Route("api/task")]
[Authorize]
public class TaskVersionController(ITaskVersionService taskVersionService) : ControllerBase
{
    private readonly ResponseDto _response = new();
    private readonly ITaskVersionService _taskVersionService = taskVersionService;

    [HttpPost("{taskId:int}/version")]
    [Authorize(Roles = UserRoles.TaskManagerRoles)]
    public async Task<IActionResult> Create(int taskId, [FromBody] CreateVersionRequest dto)
    {
        _response.Result = await _taskVersionService.CreateAsync(taskId, dto);
        return Ok(_response);
    }

    [HttpPost("{taskId:int}/version/{versionId:int}")]
    [Authorize(Roles = UserRoles.TaskManagerRoles)]
    public async Task<IActionResult> Edit(int taskId, int versionId, [FromBody] EditVersionRequest dto)
    {
        _response.Result = await _taskVersionService.EditAsync(taskId, versionId, dto);
        return Ok(_response);
    }

    [HttpPost("splitted")]
    public async Task<IActionResult> GetAll(GetTaskVersionsRequestDto dto)
    {
        _response.Result = await _taskVersionService.GetAllAsync(dto);
        return Ok(_response);
    }

    [HttpPost("full")]
    public async Task<IActionResult> GetFull(GetTaskVersionsForTestResultRequestDto dto)
    {
        _response.Result = await _taskVersionService.GetFullAsync(dto);
        return Ok(_response);
    }
    
    [HttpGet("{taskId:int}/version/{versionId:int}")]
    public async Task<IActionResult> GetFull(int taskId, int versionId)
    {
        _response.Result = await _taskVersionService.GetAsync(taskId, versionId);
        return Ok(_response);
    }
}
