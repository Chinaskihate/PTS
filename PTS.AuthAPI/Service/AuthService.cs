using Microsoft.AspNetCore.Identity;
using PTS.AuthAPI.Data;
using PTS.AuthAPI.Models;
using PTS.AuthAPI.Models.Dto;
using PTS.AuthAPI.Service.IService;

namespace PTS.AuthAPI.Service;

public class AuthService(
    AppDbContext db,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IJwtTokenGenerator tokenGenerator) : IAuthService
{
    private readonly AppDbContext _db = db;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly IJwtTokenGenerator _tokenGenerator = tokenGenerator;

    public async Task<bool> AssignRole(string email, string roleName)
    {
        var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
        if (user != null)
        {
            if (!(await _roleManager.RoleExistsAsync(roleName)))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            await _userManager.AddToRoleAsync(user, roleName);
            return true;
        }

        return false;
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
    {
        var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());
        bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
        if (user == null || !isValid)
        {
            return new LoginResponseDto()
            {
                User = null,
                Token = string.Empty
            };
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenGenerator.GenerateToken(user, roles);

        UserDto userDto = new()
        {
            Email = user.Email,
            Id = user.Id,
            TelegramId = user.TelegramId,
            PhoneNumber = user.PhoneNumber
        };

        LoginResponseDto loginResponseDto = new()
        {
            User = userDto,
            Token = token
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
                var userToReturn = _db.ApplicationUsers.First(u => u.UserName == registrationRequestDto.Email);

                UserDto userDto = new()
                {
                    Email = userToReturn.Email,
                    Id = userToReturn.Id,
                    TelegramId = userToReturn.TelegramId,
                    PhoneNumber = userToReturn.PhoneNumber
                };

                return string.Empty;
            }
            else
            {
                return result.Errors.FirstOrDefault().Description;
            }
        }
        catch (Exception ex)
        {
            return "Failed:" + ex.Message;
        }
    }
}