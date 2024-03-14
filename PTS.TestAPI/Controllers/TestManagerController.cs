using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTS.Backend.Service.IService;
using PTS.Contracts.Common;
using PTS.Contracts.Tests.Dto;
using PTS.Contracts.Users;

namespace PTS.TestAPI.Controllers;

[ApiController]
[Route("api/task")]
[Authorize]
public class TestManagerController(ITestService testService) : ControllerBase
{
    private readonly ITestService _testService = testService;

    private readonly ResponseDto _response = new();

    [HttpPost]
    [Authorize(Roles = UserRoles.TestManagerRoles)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateTestRequest dto)
    {
        _response.Result = await _testService.Create(dto);
        return Ok(_response);
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.TestManagerRoles)]
    public async Task<IActionResult> GetAllAsync()
    {
        _response.Result = await _testService.GetAllAsync();
        return Ok(_response);
    }
}
