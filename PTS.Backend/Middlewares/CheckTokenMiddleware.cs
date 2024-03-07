using Microsoft.AspNetCore.Http;
using PTS.Backend.Service.IService;

namespace PTS.Backend.Middlewares;
public class CheckTokenMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context, IAuthService authService)
    {
        var response = await authService.CheckTokenAsync();
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
