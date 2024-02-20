using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTS.Contracts.Common;
using PTS.Contracts.Users;
using PTS.Persistence.DbContexts;

namespace PTS.ThemeAPI.Controllers;
[ApiController]
[Route("api/theme")]
[Authorize]
public class ThemeController(IDbContextFactory<ThemeDbContext> _dbFactory) : ControllerBase
{
    private readonly IDbContextFactory<ThemeDbContext> _dbFactory = _dbFactory;
    private ResponseDto _response = new();

    [HttpPost]
    [Authorize(Roles = UserRoles.ThemeManagerRoles)]
    public long Create([FromBody] CreateOrUpdateThemeRequest request)
    {
        return 0;
    }

    [HttpGet("{id}")]
    public GetThemeRequest Get(long id)
    {
        return null;
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.ThemeManagerRoles)]
    public IActionResult Get()
    {
        using var context = _dbFactory.CreateDbContext();
        _response.Result = context.Themes.FirstOrDefault(t => t.Id == 1);
        return Ok(_response);
    }

    [HttpPost("{id:int}")]
    [Authorize(Roles = UserRoles.ThemeManagerRoles)]
    public GetThemeRequest Edit(int id, [FromBody] ChangeThemeRequest model)
    {
        return null;
    }
}

public class CreateOrUpdateThemeRequest
{
    public string Name { get; set; }

    public long RootId { get; set; }
}

public class ChangeThemeRequest
{
    public string Name { get; set; }

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