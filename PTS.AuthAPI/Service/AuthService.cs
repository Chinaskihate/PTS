using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PTS.AuthAPI.Service.IService;
using PTS.Backend.Exceptions.Common;
using PTS.Contracts.Auth.Dto;
using PTS.Contracts.Users;
using PTS.Mail.Helpers;
using PTS.Mail.Services;
using PTS.Persistence.DbContexts;
using PTS.Persistence.Models.Users;
using Serilog;
using System.Web;

namespace PTS.AuthAPI.Service;

public class AuthService(
    IDbContextFactory<UserDbContext> dbFactory,
    UserManager<ApplicationUser> userManager,
    IEmailService mailService,
    IJwtTokenGenerator tokenGenerator,
    ITokenStorer tokenStorer) : IAuthService
{
    private readonly IDbContextFactory<UserDbContext> _dbFactory = dbFactory;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IEmailService _mailService = mailService;
    private readonly IJwtTokenGenerator _tokenGenerator = tokenGenerator;
    private readonly ITokenStorer _tokenStorer = tokenStorer;

    public async Task<bool> CheckTokenAsync(string token)
    {
        return _tokenStorer.CheckToken(token);
    }

    public async Task<bool> RecoverAccountAsync(RecoverAccountRequest dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email)
            ?? throw new NotFoundException($"User with email {dto.Email} not found");
        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var mail = MessageTemplateHelper.CreateRecoverPasswordMessage(
            user.UserName,
            user.Email,
            user.Id,
            resetToken);
        await _mailService.SendEmailAsync(mail);
        return true;
    }

    public async Task<bool> ConfirmRecoverAccountAsync(ConfirmRecoverAccountRequest dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId)
            ?? throw new NotFoundException($"User with email {dto.UserId} not found");
        await _userManager.ResetPasswordAsync(user, HttpUtility.UrlDecode(dto.ConfirmationToken), dto.NewPassword);
        return true;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
    {
        using var context = _dbFactory.CreateDbContext();
        var user = context.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());
        bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
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
        _tokenStorer.AddOrUpdateToken(user.Id, token);

        UserDto userDto = new()
        {
            Email = user.Email,
            Id = user.Id,
            TelegramId = user.TelegramId,
            IsBanned = user.IsBanned,
            FirstName = user.FirstName,
            LastName = user.LastName,
        };

        LoginResponseDto loginResponseDto = new()
        {
            User = userDto,
            Token = token,
            Roles = roles?.ToArray() ?? new string[0],
        };

        return loginResponseDto;
    }

    public async Task<string> RegisterAsync(RegistrationRequestDto registrationRequestDto)
    {
        ApplicationUser user = new()
        {
            UserName = registrationRequestDto.Email,
            Email = registrationRequestDto.Email,
            TelegramId = registrationRequestDto.TelegramId,
            NormalizedEmail = registrationRequestDto.Email.ToUpper(),
            FirstName = registrationRequestDto.FirstName,
            LastName = registrationRequestDto.LastName,
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
                    IsBanned = userToReturn.IsBanned,
                    FirstName = userToReturn.FirstName,
                    LastName = userToReturn.LastName,
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

    public async Task RevokeTokenAsync(string userId)
    {
        _tokenStorer.RemoveToken(userId);
    }

    public async Task<LoginResponseDto> TelegramLoginAsync(string telegramId)
    {
        using var context = _dbFactory.CreateDbContext();
        var user = context.ApplicationUsers.FirstOrDefault(u => u.TelegramId == telegramId);
        if (user == null || user.IsBanned)
        {
            return new LoginResponseDto()
            {
                User = null,
                Token = string.Empty
            };
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenGenerator.GenerateToken(user, roles);
        _tokenStorer.AddOrUpdateToken(user.Id, token);

        UserDto userDto = new()
        {
            Email = user.Email,
            Id = user.Id,
            TelegramId = user.TelegramId,
            IsBanned = user.IsBanned,
            FirstName = user.FirstName,
            LastName = user.LastName,
        };

        LoginResponseDto loginResponseDto = new()
        {
            User = userDto,
            Token = token,
            Roles = roles?.ToArray() ?? new string[0],
        };

        return loginResponseDto;
    }
}