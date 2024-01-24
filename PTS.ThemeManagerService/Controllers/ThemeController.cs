using Microsoft.AspNetCore.Mvc;

namespace PTS.ThemeManagerService.Controllers;
[ApiController]
[Route("[controller]")]
public class ThemeController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<ThemeController> _logger;

    public ThemeController(ILogger<ThemeController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public long Create([FromBody] CreateThemeRequest request)
    {
        return 0;
    }

    [HttpGet("{id}")]
    public GetThemeRequest Get(long id)
    {
        return null;
    }

    [HttpPost("{id}")]
    public GetThemeRequest Change([FromBody] ChangeThemeRequest request)
    {
        return null;
    }
}

public class CreateThemeRequest
{
    public string Name { get; set; }

    public long RootId { get; set; }

    public long[] ChildIds { get; set; }

    public bool IsBanned { get; set; }
}

public class ChangeThemeRequest
{
    public string Name { get; set; }

    public long RootId { get; set; }

    public long[] ChildIds { get; set; }

    public bool IsBanned { get; set; }
}

public class GetThemeRequest
{
    public long Id { get; set; }

    public string Name { get; set; }

    public long RootId { get; set; }

    public long[] ChildIds { get; set; }

    public bool IsBanned { get; set; }
}