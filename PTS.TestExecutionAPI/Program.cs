 using PTS.Backend.Extensions;
using PTS.Backend.Mappings;
using PTS.Backend.Middlewares;
using PTS.Backend.Service;
using PTS.Backend.Service.IService;
using PTS.Backend.Utils;
using PTS.Persistence.Helpers;
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
    SD.TaskAPIBase = builder.Configuration["ServiceUrls:TaskAPI"];

    // Add services to the container.
    builder.AddCorsPTS();
    builder.Services.AddTestDbContextFactory(builder.Configuration.GetConnectionString("DefaultConnection"));
    builder.Services.AddTestMapper();
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddControllers();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddHttpClient();
    builder.Services.AddHttpClient<IAuthService, AuthService>();

    builder.Services.AddScoped<ITokenProvider, TokenProvider>();
    builder.Services.AddScoped<IBaseService, BaseService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ITaskVersionProxyService, TaskVersionProxyService>();
    builder.Services.AddScoped<ITestService, TestService>();
    builder.Services.AddScoped<ITestExecutionService, TestExecutionService>();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwagger(withBearerAuth: true);

    builder.AddAppAuthenticationPTS();
    builder.Services.AddAuthorization();

    var app = builder.Build();

    app.UseCors();
    app.UseSwaggerPTS();

    app.UseHttpsRedirection();
    app.UseMiddleware<CheckTokenMiddleware>();
    app.UseMiddleware<UserIdMiddleware>();
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