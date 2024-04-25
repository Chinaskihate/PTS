using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTS.Backend.Service.IService;
using PTS.Contracts.Common;
using PTS.Contracts.Constants;

namespace PTS.StatisticsAPI.Controllers;
[ApiController]
[Route("api/stats")]
[Authorize]
public class StatisticsController(IStatisticsService statisticsService) : ControllerBase
{
    private readonly IStatisticsService _statisticsService = statisticsService;

    private readonly ResponseDto _response = new();

    [HttpGet]
    public async Task<IActionResult> GetUserTestsStats()
    {
        _response.Result = await _statisticsService.GetUserStats(GetUserId());
        return Ok(_response);
    }

    private string GetUserId() => Request.Headers[Constants.UserIdHeader];
}
