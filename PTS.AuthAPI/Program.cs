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

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
    });
    builder.Services.AddUsersDbContextFactory(builder.Configuration.GetConnectionString("DefaultConnection"));
    builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<UserDbContext>()
        .AddDefaultTokenProviders();
    builder.AddAppAuthentication(forAuthAPI: true);
    builder.Services.AddControllers();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddSingleton<ITokenStorer, TokenStorer>();
    builder.Services.AddScoped<ITokenProvider, TokenProvider>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwagger(withBearerAuth: true);

    var app = builder.Build();

    //app.UseCors(CorsExtensions.AllowAllPolicy);
    app.UseCors();
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    MigrationHelper.ApplyUserMigration(app.Services);

    using (var scope = app.Services.CreateScope())
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var adminEmail = "admin@gmail.com";
        if ((await userManager.FindByNameAsync(adminEmail)) == null)
        {
            var admin = new ApplicationUser()
            {
                UserName = adminEmail,
                Email = adminEmail,
                NormalizedEmail = adminEmail.ToUpper(),
                FirstName = "Admin",
                LastName = "Admin"
            };
            await userManager.CreateAsync(admin, "admiN123!");

            var roleName = UserRoles.RootAdmin;
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if (!(await roleManager.RoleExistsAsync(roleName)))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            await userManager.AddToRoleAsync(admin, roleName);
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