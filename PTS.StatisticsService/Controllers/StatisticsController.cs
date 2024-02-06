using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PTS.StatisticsService.Controllers;
[ApiController]
[Authorize]
[Route("[controller]/[action]")]
public class StatisticsController : ControllerBase
{
    [HttpGet("{id}")]
    public int Get(int id)
    {
        return id;
    }
}
