using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PTS.Backend.Service.IService;
using PTS.Contracts.Tasks.Dto;
using PTS.Persistence.DbContexts;

namespace PTS.Backend.Service;
public class TaskService(
    IDbContextFactory<TaskDbContext> dbContextFactory,
    ITaskVersionService taskVersionService,
    IMapper mapper) : ITaskService
{
    private readonly IDbContextFactory<TaskDbContext> _dbContextFactory = dbContextFactory;
    private readonly ITaskVersionService _taskVersionService = taskVersionService;
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
            Type = dto.Type,
            Name = dto.Name,
            Themes = themes,
        };
        context.Tasks.Add(task);

        await context.SaveChangesAsync();

        var createdTask = await context.Tasks
            .Include(t => t.Themes)
            .Include(t => t.Versions)
            .ThenInclude(v => v.TestCases)
            .FirstAsync(t => t.Id == task.Id);
        var result = _mapper.Map<TaskDto>(createdTask);
        foreach (var version in dto.Versions)
        {
            result = await _taskVersionService.CreateAsync(task.Id, version);
        }

        return result;
    }

    public async Task<TaskDto> EditAsync(int id, EditTaskRequest dto)
    {
        using var context = _dbContextFactory.CreateDbContext();
        //var themes = await context.Themes
        //    .Where(th => dto.ThemeIds.Contains(th.Id))
        //    .ToListAsync();

        var task = await context.Tasks
            //.Include(t => t.Themes)
            .Include(t => t.Versions)
            .ThenInclude(v => v.TestCases)
            .FirstAsync(t => t.Id == id);

        //task.Themes = themes;
        task.IsEnabled = dto.IsEnabled;

        await context.SaveChangesAsync();

        var result = _mapper.Map<TaskDto>(task);
        if(dto.NewVersions != null)
        {
            foreach (var version in dto.NewVersions)
            {
                result = await _taskVersionService.CreateAsync(task.Id, version);
            }
        }

        if(dto.EditedVersions != null)
        {
            foreach (var version in dto.EditedVersions)
            {
                result = await _taskVersionService.EditAsync(task.Id, version);
            }
        }
        
        return result;
    }

    public async Task<List<TaskDto>> GetAllAsync()
    {
        using var context = _dbContextFactory.CreateDbContext();
        var tasks = await context.Tasks
            .Include(t => t.Themes)
            .Include(t => t.Versions)
            .ThenInclude(v => v.TestCases)
            .ToListAsync();

        var result = _mapper.Map<List<TaskDto>>(tasks);
        return result;
    }

    public async Task<TaskDto> GetAsync(int id)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var task = await context.Tasks
            .Include(t => t.Themes)
            .Include(t => t.Versions)
            .ThenInclude(v => v.TestCases)
            .FirstAsync(t => t.Id == id);

        var result = _mapper.Map<TaskDto>(task);
        return result;
    }
}
