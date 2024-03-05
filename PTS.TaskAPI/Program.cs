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

    // Add services to the container.
    builder.AddCors();
    builder.Services.AddTaskDbContextFactory(builder.Configuration.GetConnectionString("DefaultConnection"));
    builder.Services.AddTaskMapper();
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddControllers();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddHttpClient();
    builder.Services.AddHttpClient<IAuthService, AuthService>();

    builder.Services.AddScoped<ITokenProvider, TokenProvider>();
    builder.Services.AddScoped<IBaseService, BaseService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ITaskService, TaskService>();
    builder.Services.AddScoped<ITaskVersionService, TaskVersionService>();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwagger(withBearerAuth: true);

    builder.AddAppAuthentication();
    builder.Services.AddAuthorization();

    var app = builder.Build();

    app.UseCors();
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();
    app.UseMiddleware<CheckTokenMiddleware>();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    MigrationHelper.ApplyTaskMigration(app.Services);

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