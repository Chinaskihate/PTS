using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTS.AdminAPI.Services;
using PTS.Contracts.Common;
using PTS.Contracts.Users;
using PTS.Contracts.Users.Dto;

namespace PTS.AdminAPI.Controllers;

[Route("api/admin")]
[ApiController]
[Authorize(Roles = UserRoles.AnyAdmin)]
public class AdminController(
    IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private ResponseDto _response = new();

    [HttpGet("users")]
    public async Task<ResponseDto> Get()
    {
        _response.Result = await _userService.GetUsersAsync();
        return _response;
    }

    [HttpGet]
    [Route("users/{id}")]
    public async Task<ResponseDto> Get(string id)
    {
        try
        {
            _response.Result = await _userService.GetUserAsync(id);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }
        return _response;
    }

    [HttpPost("{id}")]
    public async Task<ResponseDto> EditAsync(string id, [FromBody] EditUserRequest model)
    {
        _response.Result = await _userService.EditUserAsync(id, model);
        return _response;
    }
}