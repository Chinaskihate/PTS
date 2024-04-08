using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTS.Backend.Extensions;
using PTS.Backend.Service.IService;
using PTS.Contracts.Common;
using PTS.Contracts.Tasks.Dto;
using PTS.Contracts.Tests.Dto;
using PTS.Contracts.Users;

namespace PTS.TestAPI.Controllers;

[ApiController]
[Route("api/test")]
[Authorize]
public class TestManagerController(ITestService testService) : ControllerBase
{
    private readonly ITestService _testService = testService;

    private readonly ResponseDto _response = new();

    [HttpPost]
    [Authorize(Roles = UserRoles.TestManagerRoles)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateTestRequest dto)
    {
        _response.Result = await _testService.Create(dto, this.GetUserId());
        return Ok(_response);
    }

    [HttpPost("filtered")]
    public async Task<IActionResult> GetAllAsync(GetTestsRequestDto dto)
    {
        _response.Result = await _testService.GetAllAsync(dto);
        return Ok(_response);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        _response.Result = await _testService.Get(id);
        return Ok(_response);
    }
}
