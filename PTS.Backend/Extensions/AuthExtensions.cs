using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PTS.Backend.Auth;
using System.Text;

namespace PTS.Backend.Extensions;
public static class AuthExtensions
{
    public static WebApplicationBuilder AddAppAuthenticationPTS(
        this WebApplicationBuilder builder,
        bool forAuthAPI = false)
    {
        var sectionPostfix = forAuthAPI ? ":JwtOptions" : "";
        var settingsSection = builder.Configuration.GetSection("ApiSettings" + sectionPostfix);
        if (forAuthAPI)
        {
            builder.Services.Configure<JwtOptions>(settingsSection);
        }

        var secret = settingsSection.GetValue<string>("Secret");
        var issuer = settingsSection.GetValue<string>("Issuer");
        var audience = settingsSection.GetValue<string>("Audience");

        var key = Encoding.ASCII.GetBytes(secret);

        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                ValidateAudience = true
            };
        });

        return builder;
    }
}
