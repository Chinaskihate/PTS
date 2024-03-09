using Microsoft.EntityFrameworkCore;
using PTS.Persistence.Models.Tests;
using PTS.Persistence.Models.Tests.Versions;

namespace PTS.Persistence.DbContexts;
public class TestDbContext : DbContext
{
    public TestDbContext() : base()
    {
        
    }

    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {

    }

    public DbSet<Test> Tests { get; set; }
    public DbSet<TestTaskVersion> TestTaskVersions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
