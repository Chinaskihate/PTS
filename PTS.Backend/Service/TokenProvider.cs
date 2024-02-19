using Microsoft.AspNetCore.Http;
using PTS.Backend.Service.IService;

namespace PTS.Backend.Service;
public class TokenProvider(IHttpContextAccessor contextAccessor) : ITokenProvider
{
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

    public string? GetToken()
    {
        string authorization = _contextAccessor.HttpContext?.Request.Headers["Authorization"];
        string? token = string.Empty;

        if (!string.IsNullOrWhiteSpace(authorization) &&
            authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            token = authorization.Substring("Bearer ".Length).Trim();
        }

        return token;
    }
}

