using Microsoft.AspNetCore.Http;
using PTS.Backend.Service.IService;

namespace PTS.Backend.Middlewares;
public class CheckTokenMiddleware(RequestDelegate next, IAuthService authService)
{
    private readonly RequestDelegate _next = next;
    private readonly IAuthService _authService = authService;

    public async Task InvokeAsync(HttpContext context)
    {
        var response = await _authService.CheckTokenAsync();
        if ((bool)response.Result)
        {
            await this._next(context);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }
    }
}
