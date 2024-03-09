using Microsoft.AspNetCore.Builder;
using PTS.Backend.Middlewares;

namespace PTS.Backend.Extensions;
public static class CustomExceptionHandlerMiddlewareExtenstions
{
    public static IApplicationBuilder UseExceptionHandlerPTS(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
}
