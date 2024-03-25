using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTS.Backend.Service.IService;
using PTS.Contracts.Common;
using PTS.Contracts.Tests.Dto;

namespace PTS.TestGeneratorService.Controllers;

[ApiController]
[Route("api/generator")]
[Authorize]
public class TestGeneratorController(ITestGeneratorService testGeneratorService) : ControllerBase
{
    private ResponseDto _response = new();
    private readonly ITestGeneratorService _testGeneratorService = testGeneratorService;
    
    [HttpPost("generate")]
    public async Task<IActionResult> Create([FromBody] GenerateTestRequest request)
    {
        _response.Result = await _testGeneratorService.GenerateTest(request);
        return Ok(_response);
    }
}