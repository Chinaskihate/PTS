using PTS.Backend.Service.IService;
using PTS.Backend.Utils;
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
            Url = SD.AuthAPIBase + "/api/auth/сheck-token",
        });
    }

    public async Task<ResponseDto?> RevokeTokenAsync(string id)
    {
        return await _baseService.SendAsync(new RequestDto
        {
            ApiType = ApiType.POST,
            Url = SD.AuthAPIBase + $"/api/auth/{id}/revoke-token",
        });
    }
}
