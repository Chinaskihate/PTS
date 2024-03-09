using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PTS.Persistence.DbContexts;
using Serilog;

namespace PTS.Persistence.Helpers;
public static class MigrationHelper
{
    public static IServiceCollection AddUsersDbContextFactory(this IServiceCollection services, string connectionString)
    {
        return services.AddDbContextFactory<UserDbContext>(options =>
        {
            options.EnableSensitiveDataLogging();
            options.UseNpgsql(connectionString)
                .LogTo(Log.Logger.Information, Microsoft.Extensions.Logging.LogLevel.Information, null);
        });
    }

    public static IServiceCollection AddTaskDbContextFactory(this IServiceCollection services, string connectionString)
    {
        return services.AddDbContextFactory<TaskDbContext>(options =>
        {
            options.EnableSensitiveDataLogging();
            options.UseNpgsql(connectionString)
                .LogTo(Log.Logger.Information, Microsoft.Extensions.Logging.LogLevel.Information, null);
        });
    }

    public static IServiceCollection AddTestDbContextFactory(this IServiceCollection services, string connectionString)
    {
        return services.AddDbContextFactory<TestDbContext>(options =>
        {
            options.EnableSensitiveDataLogging();
            options.UseNpgsql(connectionString)
                .LogTo(Log.Logger.Information, Microsoft.Extensions.Logging.LogLevel.Information, null);
        });
    }

    public static void ApplyTaskMigration(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        using var db = scope.ServiceProvider
            .GetRequiredService<IDbContextFactory<TaskDbContext>>()
            .CreateDbContext();
        if (db.Database.GetPendingMigrations().Any())
        {
            db.Database.Migrate();
        }
    }

    public static void ApplyUserMigration(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        using var db = scope.ServiceProvider
            .GetRequiredService<IDbContextFactory<UserDbContext>>()
            .CreateDbContext();
        if (db.Database.GetPendingMigrations().Any())
        {
            db.Database.Migrate();
        }
    }

    public static void ApplyTestMigration(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        using var db = scope.ServiceProvider
            .GetRequiredService<IDbContextFactory<TestDbContext>>()
            .CreateDbContext();
        if (db.Database.GetPendingMigrations().Any())
        {
            db.Database.Migrate();
        }
    }
}
