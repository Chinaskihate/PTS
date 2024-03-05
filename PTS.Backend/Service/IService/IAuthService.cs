using PTS.Contracts.Auth.Dto;
using PTS.Contracts.Common;

namespace PTS.Backend.Service.IService;
public interface IAuthService
{
    Task<ResponseDto?> CheckTokenAsync();
    Task<ResponseDto?> RevokeTokenAsync(LoginRequestDto loginRequestDto);
}
