using Microsoft.AspNetCore.Mvc;

namespace PTS.PersonRightsManagerService.Controllers;
[ApiController]
[Route("[controller]")]
public class PersonRightsController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<PersonRightsController> _logger;

    public PersonRightsController(ILogger<PersonRightsController> logger)
    {
        _logger = logger;
    }

    [HttpPost("{themeId}")]
    public void SetRights(long themeId, [FromBody] SetRightsRequest request)
    {
    }
}

public class SetRightsRequest
{
    public long[] UserIds { get; set; }
}