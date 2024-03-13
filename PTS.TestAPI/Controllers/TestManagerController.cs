using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTS.Backend.Service.IService;

namespace PTS.TestAPI.Controllers;

[ApiController]
[Route("api/task")]
[Authorize]
public class TestManagerController(ITestService testService) : ControllerBase
{
    private readonly ITestService _testService = testService;
}
