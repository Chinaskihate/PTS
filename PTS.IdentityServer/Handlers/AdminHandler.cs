using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PTS.Contracts.Users;
using System.Security.Claims;
using ApplicationUser = PTS.Persistence.Models.Users.ApplicationUser;

namespace PTS.IdentityServer.Handlers;

public class AdminHandler
{
    public static async Task<IResult> SetRole(
        [FromServices] IIdentityServerInteractionService interactionService,
        [FromServices] IHttpContextAccessor context,
        [FromServices] UserManager<ApplicationUser> userManager,
        [FromBody] SetRoleInputModel setRoleInputModel)
    {
        var currentUserPrincipal = interactionService.GetLogoutContextAsync(setRoleInputModel.UserId);
        var currentUser = await userManager.GetUserAsync(context.HttpContext.User);
        if (currentUser?.Role != (int)UserRole.Admin)
        {
            return Results.Forbid();
        }

        var userToUpdate = await userManager.FindByIdAsync(setRoleInputModel.UserId);
        userToUpdate.Role = setRoleInputModel.Role;
        await userManager.UpdateAsync(userToUpdate);

        return Results.Json(new { userToUpdate.Id });
    }

    public static async Task<IResult> SetRole2(
        [FromServices] HttpContext context,
        [FromServices] UserManager<ApplicationUser> userManager,
        [FromBody] SetRoleInputModel setRoleInputModel)
    {
        var currentUserPrincipal = context.User;
        var currentUser = await userManager.GetUserAsync(currentUserPrincipal);
        if (currentUser?.Role != (int)UserRole.Admin)
        {
            return Results.Forbid();
        }

        var userToUpdate = await userManager.FindByIdAsync(setRoleInputModel.UserId);
        userToUpdate.Role = setRoleInputModel.Role;
        await userManager.UpdateAsync(userToUpdate);

        return Results.Json(new { userToUpdate.Id });
    }

    public static async Task<IResult> SetRole3(
        [FromServices] IHttpContextAccessor context,
        [FromServices] UserManager<ApplicationUser> userManager,
        [FromBody] SetRoleInputModel setRoleInputModel)
    {
        var currentUserPrincipal = context.HttpContext.User;
        var currentUser = await userManager.GetUserAsync(currentUserPrincipal);
        if (currentUser?.Role != (int)UserRole.Admin)
        {
            return Results.Forbid();
        }

        var userToUpdate = await userManager.FindByIdAsync(setRoleInputModel.UserId);
        userToUpdate.Role = setRoleInputModel.Role;
        await userManager.UpdateAsync(userToUpdate);

        return Results.Json(new { userToUpdate.Id });
    }

    public record SetRoleInputModel(string UserId, int Role);

    public static async Task<IResult> BanUser()
    {
        return null;
    }

    public static async Task<IResult> UnbanUser()
    {
        return null;
    }

    public static async Task<IResult> GetUsers()
    {
        return null;
    }
}