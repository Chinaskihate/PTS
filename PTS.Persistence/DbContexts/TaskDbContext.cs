using Microsoft.EntityFrameworkCore;
using PTS.Contracts.Constants;
using PTS.Persistence.Models.TestCases;
using PTS.Persistence.Models.Themes;
using PTS.Persistence.Models.Versions;
using Task = PTS.Persistence.Models.Tasks.Task;

namespace PTS.Persistence.DbContexts;
public class TaskDbContext : DbContext
{
    public TaskDbContext() : base()
    {
        
    }

    public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
    {

    }

    public DbSet<Theme> Themes { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<TaskVersion> TaskVersions { get; set; }
    public DbSet<TestCase> TestCases { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Theme>().HasData(new Theme
        {
            Id = Constants.GlobalRootThemeId,
            Name = Constants.GlobalRootThemeName,
            IsBanned = false
        });
    }
}
