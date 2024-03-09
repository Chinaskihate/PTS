﻿using PTS.Contracts.Auth.Dto;

namespace PTS.AuthAPI.Service.IService;

public interface IAuthService
{
    Task<string> RegisterAsync(RegistrationRequestDto dto);
    Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
    Task<LoginResponseDto> TelegramLoginAsync(string telegramId);
    Task RevokeTokenAsync(string userId);
    Task<bool> CheckTokenAsync(string token);
}
