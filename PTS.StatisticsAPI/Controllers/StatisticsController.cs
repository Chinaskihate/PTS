using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTS.Backend.Service.IService;
using PTS.Contracts.Common;
using PTS.Contracts.Constants;
using PTS.Contracts.Users;

namespace PTS.StatisticsAPI.Controllers;
[ApiController]
[Route("api/stats")]
[Authorize]
public class StatisticsController(IStatisticsService statisticsService) : ControllerBase
{
    private readonly IStatisticsService _statisticsService = statisticsService;
    private readonly ResponseDto _response = new();

    [HttpGet("user")]
    public async Task<IActionResult> GetOwnTestsStats()
    {
        _response.Result = await _statisticsService.GetUserStats(GetUserId());
        return Ok(_response);
    }

    [HttpGet("user/{userId}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> GetUserTestsStats(string userId)
    {
        _response.Result = await _statisticsService.GetUserStats(userId);
        return Ok(_response);
    }

    [HttpGet("task/{taskId}")]
    [Authorize(Roles = UserRoles.TaskManagerRoles)]
    public async Task<IActionResult> GetTaskStats(int taskId)
    {
        _response.Result = await _statisticsService.GetTaskStats(taskId);
        return Ok(_response);
    }

    [HttpGet("test/{testId}")]
    [Authorize(Roles = UserRoles.TestManagerRoles)]
    public async Task<IActionResult> GetTestStats(int testId)
    {
        _response.Result = await _statisticsService.GetTestStats(testId);
        return Ok(_response);
    }

    private string GetUserId() => Request.Headers[Constants.UserIdHeader];
}
