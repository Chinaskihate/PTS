using PTS.Contracts.Auth.Dto;
using PTS.Contracts.Users;

namespace PTS.AuthAPI.Service.IService;

public interface IAuthService
{
    Task<string> Register(RegistrationRequestDto dto);
    Task<LoginResponseDto> Login(LoginRequestDto dto);
    Task<bool> AssignRole(AssignRoleRequestDto dto);
    Task<bool> RevokeToken(RevokeTokenDto dto);
    Task<bool> CheckToken(string token);
}
