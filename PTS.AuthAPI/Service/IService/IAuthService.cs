using PTS.AuthAPI.Models.Dto;
using PTS.Contracts.Auth;

namespace PTS.AuthAPI.Service.IService;

public interface IAuthService
{
    Task<string> Register(RegistrationRequestDto registrationRequestDto);
    Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    Task<bool> AssignRole(string email, string roleName);
    Task<bool> RevokeToken(LoginRequestDto loginRequestDto);
    Task<bool> CheckToken(string token);
}
