using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTS.Backend.Service.IService;
using PTS.Contracts.Common;
using PTS.Contracts.TestCases.Dto;
using PTS.Contracts.Users;

namespace PTS.TaskAPI.Controllers;

[ApiController]
[Route("api/task")]
[Authorize]
public class TestCaseController(ITestCaseService testCaseService) : ControllerBase
{
    private readonly ResponseDto _response = new();
    private readonly ITestCaseService _testCaseService = testCaseService;

    [HttpPost("{taskId:int}/version/{versionId:int}")]
    [Authorize(Roles = UserRoles.TaskManagerRoles)]
    public async Task<IActionResult> Create(int taskId, int versionId, [FromBody] CreateTestCaseRequest dto)
    {
        _response.Result = await _testCaseService.CreateAsync(taskId, versionId, dto);
        return Ok(_response);
    }

    [HttpPost("{taskId:int}/version/{versionId:int}/case/{testCaseId:int}")]
    [Authorize(Roles = UserRoles.TaskManagerRoles)]
    public async Task<IActionResult> Create(int taskId, int versionId, int testCaseId, [FromBody] EditTestCaseRequest dto)
    {
        _response.Result = await _testCaseService.EditAsync(taskId, versionId, testCaseId, dto);
        return Ok(_response);
    }
}

