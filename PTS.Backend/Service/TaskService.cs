using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PTS.Backend.Service.IService;
using PTS.Contracts.Tasks.Dto;
using PTS.Persistence.DbContexts;
using PTS.Persistence.Models.Versions;

namespace PTS.Backend.Service;
public class TaskService(
    IDbContextFactory<TaskDbContext> dbContextFactory,
    IMapper mapper) : ITaskService
{
    private readonly IDbContextFactory<TaskDbContext> _dbContextFactory = dbContextFactory;
    private readonly IMapper _mapper = mapper;

    public async Task<TaskDto> CreateAsync(CreateTaskRequestDto dto)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var themes = await context.Themes
            .Where(th => dto.ThemeIds.Contains(th.Id))
            .ToListAsync();
        var task = new Persistence.Models.Tasks.Task
        {
            IsEnabled = dto.IsEnabled,
            AvgTimeInMin = dto.AvgTimeInMin,
            Complexity = dto.Complexity,
            Name = dto.Name,
            Themes = themes,
        };
        context.Tasks.Add(task);

        var version = new TaskVersion
        {
            Description = dto.Description,
            InputCondition = dto.InputCondition,
            OutputCondition = dto.OutputCondition,
            ProgrammingLanguage = (int)dto.ProgrammingLanguage,
            Task = task
        };
        context.TaskVersions.Add(version);

        await context.SaveChangesAsync();

        var createdTask = await context.Tasks
            .Include(t => t.Versions)
            .FirstAsync(t => t.Id == task.Id);
        var result = _mapper.Map<TaskDto>(createdTask);
        return result;
    }
}
