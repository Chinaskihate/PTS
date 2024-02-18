using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PTS.Contracts.Users;
using PTS.Persistence.Models.Users;
using PTS.Persistence.Services.Users;
using Serilog;
using System.Security.Claims;
using ApplicationUser = PTS.Persistence.Models.Users.ApplicationUser;

namespace PTS.IdentityServer.Handlers;

public class AccountHandler
{
    public static async Task<IResult> LoginAsync([FromServices] IIdentityServerInteractionService interactionService,
        [FromServices] IServerUrls serverUrls,
        [FromServices] SignInManager<ApplicationUser> signInManager,
        [FromServices] UserManager<ApplicationUser> userManager,
        [FromBody] LoginInputModel input)
    {
        var returnUrl =
            input.ReturnUrl != null && await interactionService.GetAuthorizationContextAsync(input.ReturnUrl) != null
                ? input.ReturnUrl
                : serverUrls.BaseUrl;

        var user = await userManager.FindByEmailAsync(input.Email);
        var result = await signInManager.PasswordSignInAsync(user, input.Password, input.IsPersistent, true);

        return result.Succeeded ? Results.Json(new { returnUrl }) : Results.BadRequest(result.ToString());
    }

    public record LoginInputModel(string Email, string Password, string? ReturnUrl, bool IsPersistent = false);

    public static async Task<IResult> LogoutAsync([FromServices] IIdentityServerInteractionService interactionService,
        [FromServices] SignInManager<ApplicationUser> signInManager,
        [FromBody] LogoutInputModel input)
    {
        var request = await interactionService.GetLogoutContextAsync(input.LogoutId);

        await signInManager.SignOutAsync();

        return Results.Json(new
        {
            iFrameUrl = request.SignOutIFrameUrl,
            postLogoutRedirectUri = request.PostLogoutRedirectUri
        });
    }

    public record LogoutInputModel(string? LogoutId);

    public static async Task<IResult> RegisterAsync(
        [FromServices] SignInManager<ApplicationUser> signInManager,
        [FromServices] UserManager<ApplicationUser> userManager,
        [FromBody] RegisterInputModel input)
    {
        var user = new ApplicationUser
        {
            UserName = input.UserName,
            TelegramId = input.TelegramId,
            Email = input.Email
        };

        var result = await userManager.CreateAsync(user, input.Password);
        if (result.Succeeded)
        {
            await signInManager.SignInAsync(user, false);
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

    public record SetRoleInputModel(string UserId, int Role);
}
