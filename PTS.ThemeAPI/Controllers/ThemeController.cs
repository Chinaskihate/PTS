using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTS.Backend.Service.IService;
using PTS.Contracts.Common;
using PTS.Contracts.Theme.Dto;
using PTS.Contracts.Users;

namespace PTS.ThemeAPI.Controllers;
[ApiController]
[Route("api/theme")]
[Authorize]
public class ThemeController(IThemeService themeService) : ControllerBase
{
    private ResponseDto _response = new();
    private readonly IThemeService _themeService = themeService;

    [HttpPost]
    [Authorize(Roles = UserRoles.ThemeManagerRoles)]
    public async Task<IActionResult> Create([FromBody] CreateThemeRequest request)
    {
        _response.Result = await _themeService.CreateThemeAsync(request);
        return Ok(_response);
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableThemes()
    {
        _response.Result = await _themeService.GetThemes(true);
        return Ok(_response);
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.ThemeManagerRoles)]
    public async Task<IActionResult> Get()
    {
        _response.Result = await _themeService.GetThemes();
        return Ok(_response);
    }

    [HttpPost("{id:int}")]
    [Authorize(Roles = UserRoles.ThemeManagerRoles)]
    public async Task<IActionResult> Edit(int id, [FromBody] EditThemeRequest model)
    {
        _response.Result = await _themeService.EditThemeAsync(id, model);
        return Ok(_response);
    }
}
