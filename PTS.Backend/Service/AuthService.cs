using PTS.Backend.Service.IService;
using PTS.Backend.Utils;
using PTS.Contracts.Auth.Dto;
using PTS.Contracts.Common;

namespace PTS.Backend.Service;
public class AuthService(IBaseService baseService) : IAuthService
{
    private readonly IBaseService _baseService = baseService;

    public async Task<ResponseDto?> CheckTokenAsync()
    {
        return await _baseService.SendAsync(new RequestDto
        {
            ApiType = ApiType.POST,
            Url = SD.AuthAPIBase + "/api/auth/CheckToken",
        });
    }

    public async Task<ResponseDto?> RevokeTokenAsync(LoginRequestDto loginRequestDto)
    {
        return await _baseService.SendAsync(new RequestDto
        {
            ApiType = ApiType.POST,
            Url = SD.AuthAPIBase + "/api/auth/RevokeToken",
        });
    }
}
