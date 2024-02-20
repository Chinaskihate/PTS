using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PTS.Persistence.DbContexts;

namespace PTS.Persistence.Helpers;
public static class MigrationHelper
{
    public static IServiceCollection AddUsersDbContextFactory(this IServiceCollection services, string connectionString)
    {
        return services.AddDbContextFactory<UserDbContext>(options =>
        {
            options.EnableSensitiveDataLogging();
            options.UseNpgsql(connectionString);
        });
    }

    public static IServiceCollection AddThemeDbContextFactory(this IServiceCollection services, string connectionString)
    {
        return services.AddDbContextFactory<ThemeDbContext>(options =>
        {
            options.EnableSensitiveDataLogging();
            options.UseNpgsql(connectionString);
        });
    }

    public static void ApplyThemeMigration(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        using var db = scope.ServiceProvider
            .GetRequiredService<IDbContextFactory<ThemeDbContext>>()
            .CreateDbContext();
        if (db.Database.GetPendingMigrations().Count() > 0)
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
        if (db.Database.GetPendingMigrations().Count() > 0)
        {
            db.Database.Migrate();
        }
    }
}
