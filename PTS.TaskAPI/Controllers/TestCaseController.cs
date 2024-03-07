﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTS.Backend.Service.IService;
using PTS.Contracts.Common;
using PTS.Contracts.Users;
using PTS.Contracts.Versions.Dto;

namespace PTS.TaskAPI.Controllers;

[ApiController]
[Route("api/task")]
[Authorize]
public class TestCaseController(ITaskVersionService taskVersionService) : ControllerBase
{
    private readonly ResponseDto _response = new();
    private readonly ITaskVersionService _taskVersionService = taskVersionService;

    [HttpPost("{taskId:int}/version/{versionId:int}")]
    [Authorize(Roles = UserRoles.TaskManagerRoles)]
    public async Task<IActionResult> Create(int taskId, int versionId, [FromBody] CreateVersionRequest dto)
    {
        _response.Result = await _taskVersionService.CreateAsync(taskId, dto);
        return Ok(_response);
    }

    [HttpPost("{taskId:int}/version/{versionId:int}/case/{testCaseId:int}")]
    [Authorize(Roles = UserRoles.TaskManagerRoles)]
    public async Task<IActionResult> Create(int taskId, int versionId, int testCaseId, [FromBody] EditVersionRequest dto)
    {
        _response.Result = await _taskVersionService.EditAsync(taskId, versionId, dto);
        return Ok(_response);
    }
}

