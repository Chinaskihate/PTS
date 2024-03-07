using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTS.Backend.Service.IService;
using PTS.Contracts.Auth.Dto;
using PTS.Contracts.Common;
using PTS.Contracts.Users;
using IAuthService = PTS.AuthAPI.Service.IService.IAuthService;

namespace PTS.AuthAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
    IAuthService authService,
    ITokenProvider tokenProvider) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    protected ResponseDto _response = new();

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
    {
        var errorMessage = await _authService.RegisterAsync(model);
        if (!string.IsNullOrEmpty(errorMessage))
        {
            _response.IsSuccess = false;
            _response.Message = errorMessage;
            return BadRequest(_response);
        }

        return Ok(_response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
    {
        var loginResponse = await _authService.LoginAsync(model);
        if (loginResponse.User == null)
        {
            _response.IsSuccess = false;
            _response.Message = "auth failed";
            return BadRequest(_response);
        }

        _response.Result = loginResponse;
        return Ok(_response);
    }

    [HttpPost("{id}/RevokeToken")]
    [Authorize(Roles = UserRoles.AnyAdmin)]
    public async Task<IActionResult> RevokeToken(string id)
    {
        var tokenRevoked = await _authService.RevokeTokenAsync(id);
        if (!tokenRevoked)
        {
            _response.IsSuccess = false;
            _response.Message = "user not found";
            return NotFound(_response);
        }

        _response.Result = tokenRevoked;
        return Ok(_response);
    }

    [HttpPost("CheckToken")]
    public async Task<IActionResult> CheckToken()
    {
        _response.Result = await CheckCurrentToken();
        return Ok(_response);
    }

    private async Task<bool> CheckCurrentToken()
    {
        return await _authService.CheckTokenAsync(_tokenProvider.GetToken());
    }
}
