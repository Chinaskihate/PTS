using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTS.Backend.Service.IService;
using PTS.Contracts.Common;
using PTS.Contracts.PTSTestResults;
using PTS.Backend.Extensions;

namespace PTS.TestExecutionAPI.Controllers;

[ApiController]
[Route("api/execution")]
[Authorize]
public class TestExecutionController(ITestExecutionService testExecutionService) : ControllerBase
{
    private readonly ITestExecutionService _testExecutionService = testExecutionService;

    private readonly ResponseDto _response = new();

    [HttpPost("start")]
    public async Task<IActionResult> StartAsync([FromBody] StartTestDto dto)
    {
        _response.Result = await _testExecutionService.StartTestAsync(dto, this.GetUserId());
        return Ok(_response);
    }

    [HttpGet]
    public async Task<IActionResult> GetUserTests()
    {
        _response.Result = await _testExecutionService.GetUserTestsAsync(this.GetUserId());
        return Ok(_response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTestStatusAsync(int testResultId)
    {
        _response.Result = await _testExecutionService.GetTestStatusAsync(testResultId, this.GetUserId());
        return Ok(_response);
    }

    [HttpPost("submit")]
    public async Task<IActionResult> SubmitTaskAsync(SubmitTaskDto dto)
    {
        _response.Result = await _testExecutionService.SubmitTaskAsync(dto, this.GetUserId());
        return Ok(_response);
    }
}