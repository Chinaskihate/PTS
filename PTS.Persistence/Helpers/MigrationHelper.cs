﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PTS.Persistence.DbContexts;

namespace PTS.Persistence.Helpers;
public static class MigrationHelper
{
    public static IServiceCollection AddUsersDbContextFactory(this IServiceCollection services, string connectionString)
    {
        return services.AddDbContextFactory<UserDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
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
