using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PTS.AuthAPI.Service;
using PTS.AuthAPI.Service.IService;
using PTS.Backend.Extensions;
using PTS.Backend.Service;
using PTS.Backend.Service.IService;
using PTS.Contracts.Users;
using PTS.Persistence.DbContexts;
using PTS.Persistence.Helpers;
using PTS.Persistence.Models.Users;
using Serilog;
using AuthService = PTS.AuthAPI.Service.AuthService;
using IAuthService = PTS.AuthAPI.Service.IService.IAuthService;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(ctx.Configuration));

    // Add services to the container.

    builder.AddCorsPTS();
    builder.Services.AddUsersDbContextFactory(builder.Configuration.GetConnectionString("DefaultConnection"));
    builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<UserDbContext>()
        .AddDefaultTokenProviders();
    builder.AddAppAuthenticationPTS(forAuthAPI: true);
    builder.Services.AddControllers();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddSingleton<ITokenStorer, TokenStorer>();
    builder.Services.AddScoped<ITokenProvider, TokenProvider>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwagger(withBearerAuth: true);

    var app = builder.Build();

    app.UseCors();
    app.UseSwaggerPTS();
    app.UseExceptionHandlerPTS();

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    MigrationHelper.ApplyUserMigration(app.Services);

    using (var scope = app.Services.CreateScope())
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await CreateUserAsync("root@gmail.com", "rooT12345!", "RootAdmin", UserRoles.RootAdmin);
        await CreateUserAsync("admin@gmail.com", "admiN12345!", "Admin", UserRoles.Admin);
        await CreateUserAsync("theme@gmail.com", "themE12345!", "Theme", UserRoles.ThemeManager);
        await CreateUserAsync("task@gmail.com", "tasK12345!", "Task", UserRoles.TaskManager);
        await CreateUserAsync("test@gmail.com", "tesT12345!", "Test", UserRoles.TestManager);
        await CreateUserAsync("tgbot@gmail.com", "tgboT12345!", "TgBot", UserRoles.TelegramBot);

        async Task CreateUserAsync(string email, string password, string name, string roleName)
        {
            if ((await userManager.FindByNameAsync(email)) == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = email,
                    Email = email,
                    NormalizedEmail = email.ToUpper(),
                    FirstName = name,
                    LastName = name
                };
                await userManager.CreateAsync(user, password);

                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (!(await roleManager.RoleExistsAsync(roleName)))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }

                await userManager.AddToRoleAsync(user, roleName);
            }
        }
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}