using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using PTS.IdentityServer.Data;
using PTS.IdentityServer.Handlers;
using PTS.IdentityServer.Models;
using Serilog;
using System.Reflection;

namespace PTS.IdentityServer;
internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
        IdentityModelEventSource.ShowPII = true;

        builder.Host.UseSerilog((ctx, lc) => lc
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
            .Enrich.FromLogContext()
            .ReadFrom.Configuration(ctx.Configuration));

        builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, dbContextOptionsBuilder) =>
        {
            dbContextOptionsBuilder.UseNpgsql(
                serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("Identity"),
                NpgsqlOptionsAction);
        });

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddIdentityServer(identityServerOptions =>
        {
            identityServerOptions.UserInteraction.LoginUrl = "/account/login";
            identityServerOptions.UserInteraction.LoginReturnUrlParameter = "returnUrl";
            identityServerOptions.UserInteraction.LogoutUrl = "/account/logout";
            identityServerOptions.UserInteraction.LogoutIdParameter = "logoutId";
        })
            .AddAspNetIdentity<ApplicationUser>()
            .AddConfigurationStore(configurationStoreOptions =>
            {
                configurationStoreOptions.ResolveDbContextOptions = ResolveDbContextOptions;
            })
            .AddOperationalStore(operationalStoreOptions =>
            {
                operationalStoreOptions.ResolveDbContextOptions = ResolveDbContextOptions;
            });

        return builder.Build();
    }

    public static async Task<WebApplication> ConfigurePipelineAsync(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseIdentityServer();

        app.MapPost("/api/login", AccountHandler.LoginAsync);
        app.MapPost("/api/logout", AccountHandler.LogoutAsync);

        app.MapFallbackToFile("index.html");

        if (app.Environment.IsDevelopment())
        {
            using var scope = app.Services.CreateScope();

            await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();
            await scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.MigrateAsync();
            await scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.MigrateAsync();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (await userManager.FindByNameAsync("thomas.clark") == null)
            {
                await userManager.CreateAsync(
                    new ApplicationUser
                    {
                        UserName = "thomas.clark",
                        Email = "thomas.clark@example.com",
                        TelegramId = "123",
                        Role = 1
                    }, "Pa55w0rd!");
            }

            var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

            if (!await configurationDbContext.Clients.AnyAsync())
            {
                await configurationDbContext.Clients.AddRangeAsync(
                    new Client
                    {
                        ClientId = "4ecc4153-daf9-4eca-8b60-818a63637a81",
                        ClientSecrets = new List<Secret> { new("secret".Sha512()) },
                        ClientName = "Web Application",
                        AllowedGrantTypes = GrantTypes.Code,
                        AllowedScopes = new List<string> { "openid", "profile", "email" },
                        RedirectUris = new List<string> { "https://webapplication:7002/signin-oidc", "https://localhost:7107/signin-oidc" },
                        PostLogoutRedirectUris = new List<string> { "https://webapplication:7002/signout-callback-oidc" }
                    }.ToEntity());

                await configurationDbContext.SaveChangesAsync();
            }

            if (!await configurationDbContext.IdentityResources.AnyAsync())
            {
                await configurationDbContext.IdentityResources.AddRangeAsync(
                    new IdentityResources.OpenId().ToEntity(),
                    new IdentityResources.Profile().ToEntity(),
                    new IdentityResources.Email().ToEntity());

                await configurationDbContext.SaveChangesAsync();
            }

            app.UseDeveloperExceptionPage();
        }

        return app;
    }
    private static void NpgsqlOptionsAction(NpgsqlDbContextOptionsBuilder npgsqlDbContextOptionsBuilder)
    {
        npgsqlDbContextOptionsBuilder.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name);
    }

    private static void ResolveDbContextOptions(IServiceProvider serviceProvider, DbContextOptionsBuilder dbContextOptionsBuilder)
    {
        dbContextOptionsBuilder.UseNpgsql(
            serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("IdentityServer"),
            NpgsqlOptionsAction);
    }
}
