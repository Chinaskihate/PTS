using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace PTS.Backend.Extensions;
public static class CorsExtensions
{
    public const string AllowAllPolicy = "AllowAll";

    public static WebApplicationBuilder AddCors(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(AllowAllPolicy, policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
            });
        });
        return builder;
    }
}
