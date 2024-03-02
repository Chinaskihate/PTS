using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PTS.AuthAPI.Models.Dto;
using PTS.AuthAPI.Service.IService;
using PTS.Contracts.Auth;
using PTS.Contracts.Users;
using PTS.Persistence.DbContexts;
using PTS.Persistence.Models.Users;
using Serilog;

namespace PTS.AuthAPI.Service;

public class AuthService(
    IDbContextFactory<UserDbContext> dbFactory,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IJwtTokenGenerator tokenGenerator,
    ITokenStorer tokenStorer) : IAuthService
{
    private readonly IDbContextFactory<UserDbContext> _dbFactory = dbFactory;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly IJwtTokenGenerator _tokenGenerator = tokenGenerator;
    private readonly ITokenStorer _tokenStorer = tokenStorer;

    public async Task<bool> AssignRole(string email, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null)
        {
            if (!(await _roleManager.RoleExistsAsync(roleName)) && UserRoles.AllRoles.Contains(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
            
            await _userManager.AddToRoleAsync(user, roleName);
            return true;
        }

        return false;
    }

    public async Task<bool> CheckToken(string token)
    {
        return _tokenStorer.CheckToken(token);
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
    {
        using var context = _dbFactory.CreateDbContext();
        var user = context.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());
        bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
        //context.Entry(user).Reload();
        if (user == null || !isValid || user.IsBanned)
        {
            return new LoginResponseDto()
            {
                User = null,
                Token = string.Empty
            };
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenGenerator.GenerateToken(user, roles);
        _tokenStorer.AddOrUpdateToken(user, token);

        UserDto userDto = new()
        {
            Email = user.Email,
            Id = user.Id,
            TelegramId = user.TelegramId,
            PhoneNumber = user.PhoneNumber,
            IsBanned = user.IsBanned,
        };

        LoginResponseDto loginResponseDto = new()
        {
            User = userDto,
            Token = token,
            Roles = roles?.ToArray() ?? new string[0],
        };

        return loginResponseDto;
    }

    public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
    {
        ApplicationUser user = new()
        {
            UserName = registrationRequestDto.Email,
            Email = registrationRequestDto.Email,
            TelegramId = registrationRequestDto.TelegramId,
            NormalizedEmail = registrationRequestDto.Email.ToUpper(),
            PhoneNumber = registrationRequestDto.PhoneNumber
        };

        try
        {
            var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
            if (result.Succeeded)
            {
                using var context = _dbFactory.CreateDbContext();
                var userToReturn = context.ApplicationUsers.First(u => u.UserName == registrationRequestDto.Email);

                UserDto userDto = new()
                {
                    Email = userToReturn.Email,
                    Id = userToReturn.Id,
                    TelegramId = userToReturn.TelegramId,
                    PhoneNumber = userToReturn.PhoneNumber,
                    IsBanned = userToReturn.IsBanned,
                };

                return string.Empty;
            }
            else
            {
                Log.Error(result.Errors.FirstOrDefault().Description);
                return result.Errors.FirstOrDefault().Description;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return "Failed:" + ex.Message;
        }
    }

    public async Task<bool> RevokeToken(LoginRequestDto loginRequestDto)
    {
        using var context = _dbFactory.CreateDbContext();
        var user = context.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());
        return user == null ? false : _tokenStorer.RemoveToken(user);
    }
}