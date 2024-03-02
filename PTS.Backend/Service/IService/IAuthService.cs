using PTS.Contracts.Auth;
using PTS.Contracts.Common;

namespace PTS.Backend.Service.IService;
public interface IAuthService
{
    Task<ResponseDto?> CheckTokenAsync();
    Task<ResponseDto?> RevokeTokenAsync(LoginRequestDto loginRequestDto);
}
