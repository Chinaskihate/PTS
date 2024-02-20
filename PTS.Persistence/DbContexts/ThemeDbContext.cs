using Microsoft.EntityFrameworkCore;
using PTS.Persistence.Models.Themes;

namespace PTS.Persistence.DbContexts;
public class ThemeDbContext : DbContext
{
    public ThemeDbContext() : base()
    {
        
    }

    public ThemeDbContext(DbContextOptions<ThemeDbContext> options) : base(options)
    {

    }

    public DbSet<Theme> Themes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Theme>().HasData(new Theme
        {
            Id = 1,
            Name = "RootTheme",
            IsBanned = false
        });
    }
}
