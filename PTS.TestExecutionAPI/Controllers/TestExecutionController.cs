using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTS.Backend.Service.IService;
using PTS.Contracts.Common;
using PTS.Contracts.Constants;
using PTS.Contracts.PTSTestResults;

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
        _response.Result = await _testExecutionService.StartTestAsync(dto, GetUserId());
        return Ok(_response);
    }

    [HttpGet]
    public async Task<IActionResult> GetUserTests()
    {
        _response.Result = await _testExecutionService.GetUserTestsAsync(GetUserId());
        return Ok(_response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTestStatusAsync(int testResultId)
    {
        _response.Result = await _testExecutionService.GetTestStatusAsync(testResultId, GetUserId());
        return Ok(_response);
    }

    [HttpPost("submit")]
    public async Task<IActionResult> SubmitTaskAsync(SubmitTaskDto dto)
    {
        _response.Result = await _testExecutionService.SubmitTaskAsync(dto, GetUserId());
        return Ok(_response);
    }

    private string GetUserId() => Request.Headers[Constants.UserIdHeader];
}