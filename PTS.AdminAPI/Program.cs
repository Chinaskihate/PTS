using Microsoft.AspNetCore.Identity;
using PTS.AdminAPI.Services;
using PTS.Backend.Extensions;
using PTS.Backend.Mappings;
using PTS.Backend.Middlewares;
using PTS.Backend.Service;
using PTS.Backend.Service.IService;
using PTS.Backend.Utils;
using PTS.Persistence.DbContexts;
using PTS.Persistence.Helpers;
using PTS.Persistence.Models.Users;
using Serilog;

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

    SD.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"];

    // Add services to the container.
    builder.AddCorsPTS();
    builder.Services.AddUsersDbContextFactory(builder.Configuration.GetConnectionString("DefaultConnection"));
    builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<UserDbContext>()
        .AddDefaultTokenProviders();
    builder.Services.AddUserMapper();
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddControllers();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddHttpClient();
    builder.Services.AddHttpClient<IAuthService, AuthService>();

    builder.Services.AddScoped<ITokenProvider, TokenProvider>();
    builder.Services.AddScoped<IBaseService, BaseService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IUserService, UserService>();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwagger(withBearerAuth: true);

    builder.AddAppAuthenticationPTS();
    builder.Services.AddAuthorization();

    var app = builder.Build();

    app.UseCors();
    app.UseSwaggerPTS();
    app.UseMiddleware<CheckTokenMiddleware>();
    app.UseExceptionHandlerPTS();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

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