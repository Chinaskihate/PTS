﻿using Microsoft.EntityFrameworkCore;
using PTS.Contracts.Constants;
using PTS.Persistence.Models.Themes;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Theme>().HasData(new Theme
        {
            Id = Constants.GlobalRootThemeId,
            Name = "RootTheme",
            IsBanned = false
        });
    }
}