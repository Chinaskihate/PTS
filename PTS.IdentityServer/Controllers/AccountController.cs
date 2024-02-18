using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PTS.Contracts.Users;
using Serilog;
using ApplicationUser = PTS.Persistence.Models.Users.ApplicationUser;

namespace PTS.IdentityServer.Controllers;

[ApiController]
[Route("api/[action]")]
public class AccountController : ControllerBase
{
    private readonly IIdentityServerInteractionService _interactionService;
    private readonly IServerUrls _serverUrls;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(
        IIdentityServerInteractionService interactionService,
        IServerUrls serverUrls,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
    {
        _interactionService = interactionService;
        _serverUrls = serverUrls;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpPost("login")]
    public async Task<IResult> LoginAsync(
        [FromBody] LoginInputModel input)
    {
        var returnUrl =
            input.ReturnUrl != null && await _interactionService.GetAuthorizationContextAsync(input.ReturnUrl) != null
                ? input.ReturnUrl
                : _serverUrls.BaseUrl;

        var user = await _userManager.FindByEmailAsync(input.Email);
        var result = await _signInManager.PasswordSignInAsync(user, input.Password, input.IsPersistent, true);

        return result.Succeeded ? Results.Json(new { returnUrl }) : Results.BadRequest(result.ToString());
    }

    public record LoginInputModel(string Email, string Password, string? ReturnUrl, bool IsPersistent = false);

    [HttpPost("logout")]
    public async Task<IResult> LogoutAsync(
        [FromBody] LogoutInputModel input)
    {
        var request = await _interactionService.GetLogoutContextAsync(input.LogoutId);

        await _signInManager.SignOutAsync();

        return Results.Json(new
        {
            iFrameUrl = request.SignOutIFrameUrl,
            postLogoutRedirectUri = request.PostLogoutRedirectUri
        });
    }

    public record LogoutInputModel(string? LogoutId);

    [HttpPost("register")]
    public async Task<IResult> RegisterAsync(
        [FromBody] RegisterInputModel input)
    {
        var user = new ApplicationUser
        {
            UserName = input.UserName,
            TelegramId = input.TelegramId,
            Email = input.Email
        };

        var result = await _userManager.CreateAsync(user, input.Password);
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);
            return Results.Json(new
            {
                input.ReturnUrl
            });
        }

        return Results.BadRequest(result.ToString());
    }

    public record RegisterInputModel(
        string Email,
        string UserName,
        string Password,
        string ConfirmPassword,
        string? TelegramId,
        string? ReturnUrl);

    [HttpPost("role2")]
    public async Task<IResult> SetRole2(
        [FromBody] SetRoleInputModel setRoleInputModel)
    {
        try
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.Role != (int)UserRole.Admin)
            {
                return Results.Forbid();
            }

            var userToUpdate = await _userManager.FindByIdAsync(setRoleInputModel.UserId);
            userToUpdate.Role = setRoleInputModel.Role;
            await _userManager.UpdateAsync(userToUpdate);

            return Results.Json(new { UserId = userToUpdate.Id });
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
        }

    }

    public async Task<IResult> SetRole3(
        [FromBody] SetRoleInputModel setRoleInputModel)
    {
        return Results.Json(new { number = setRoleInputModel.Role });
    }

    public record SetRoleInputModel(string UserId, int Role);
}
