using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PTS.Backend.Exceptions.Common;
using PTS.Backend.Service.IService;
using PTS.Contracts.Users;
using PTS.Contracts.Users.Dto;
using PTS.Persistence.Models.Users;

namespace PTS.AdminAPI.Services;

public class UserService(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IMapper mapper,
    IAuthService authService) : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly IMapper _mapper = mapper;
    private readonly IAuthService _authService = authService;

    public async Task<UserDto> EditUserAsync(string id, EditUserRequest dto)
    {
        var user = await _userManager.FindByIdAsync(id) ?? throw new NotFoundException($"User {id} not found.");

        await AssignRoles(user, dto.Roles);
        if (dto.IsBanned)
        {
            await BanAsync(user);
        }
        else
        {
            await UnbanAsync(user);
        }

        var userDto = _mapper.Map<UserDto>(user);
        userDto.Roles = (await _userManager.GetRolesAsync(user)).ToArray();
        return userDto;
    }

    private async Task BanAsync(ApplicationUser user)
    {
        user.IsBanned = true;
        var response = await _authService.RevokeTokenAsync(user.Id);
        if (!response.IsSuccess)
        {
            throw new Exception(response.Message);
        }
    }

    private async Task UnbanAsync(ApplicationUser user)
    {
        user.IsBanned = false;
    }

    private async Task<bool> AssignRoles(ApplicationUser user, string[] roles)
    {
        var userRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, userRoles);

        if (!roles.IsNullOrEmpty())
        {
            foreach (var role in roles)
            {
                var roleName = role.ToUpper();
                if (!(await _roleManager.RoleExistsAsync(roleName)) && UserRoles.AllRoles.Contains(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }

                await _userManager.AddToRoleAsync(user, roleName);
            }
        }

        return true;
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        var dtos = new List<UserDto>();
        foreach (var user in users)
        {
            var dto = _mapper.Map<UserDto>(user);
            dto.Roles = (await _userManager.GetRolesAsync(user)).ToArray();
            dtos.Add(dto);
        }

        return dtos;
    }

    public async Task<UserDto> GetUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        var dto = _mapper.Map<UserDto>(user);
        dto.Roles = [.. (await _userManager.GetRolesAsync(user))];

        return dto;
    }
}
