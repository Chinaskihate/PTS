using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PTS.Backend.Exceptions.Common;
using PTS.Backend.Service.IService;
using PTS.Contracts.PTSTestResults;
using PTS.Contracts.Tasks.Dto;
using PTS.Contracts.Theme.Dto;
using PTS.Contracts.Versions.Dto;
using PTS.Persistence.DbContexts;
using PTS.Persistence.Models.Versions;

namespace PTS.Backend.Service;
public class TaskVersionService(
    IDbContextFactory<TaskDbContext> dbContextFactory,
    ITestCaseService testCaseService,
    IThemeService themeService,
    IMapper mapper) : ITaskVersionService
{
    private readonly IDbContextFactory<TaskDbContext> _dbContextFactory = dbContextFactory;
    private readonly ITestCaseService _testCaseService = testCaseService;
    private readonly IThemeService _themeService = themeService;
    private readonly IMapper _mapper = mapper;

    public async Task<TaskDto> CreateAsync(int taskId, CreateVersionRequest dto)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var task = context.Tasks
            .Include(t => t.Themes)
            .Include(t => t.Versions)
            .ThenInclude(v => v.TestCases)
            .FirstOrDefault(t => t.Id == taskId) ?? throw new NotFoundException($"Task {taskId} not found");
        var version = new TaskVersion()
        {
            Description = dto.Description,
            InputCondition = dto.InputCondition,
            OutputCondition = dto.OutputCondition,
            ProgrammingLanguage = (int)dto.ProgrammingLanguage,
            Task = task,
            CodeTemplate = JsonConvert.SerializeObject(dto.CodeTemplate)
        };

        context.TaskVersions.Add(version);

        await context.SaveChangesAsync();

        TaskDto result = _mapper.Map<TaskDto>(task);
        foreach (var testCase in dto.TestCases)
        {
            result = await _testCaseService.CreateAsync(taskId, version.Id, testCase);
        }

        return result;
    }

    public async Task<TaskDto> EditAsync(int taskId, int versionId, EditVersionRequest dto)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var task = context.Tasks
            .Include(t => t.Themes)
            .Include(t => t.Versions)
            .ThenInclude(v => v.TestCases)
            .FirstOrDefault(t => t.Id == taskId) ?? throw new NotFoundException($"Task {taskId} not found");
        var version = task.Versions
            .FirstOrDefault(v => v.Id == versionId) ?? throw new NotFoundException($"Version {versionId} not found");

        version.Description = dto.Description;
        version.InputCondition = dto.InputCondition;
        version.OutputCondition = dto.OutputCondition;
        version.CodeTemplate = JsonConvert.SerializeObject(dto.CodeTemplate);

        await context.SaveChangesAsync();

        TaskDto result = _mapper.Map<TaskDto>(task);
        foreach (var testCase in dto.NewTestCases)
        {
            result = await _testCaseService.CreateAsync(taskId, version.Id, testCase);
        }

        foreach (var testCase in dto.EditedTestCases)
        {
            result = await _testCaseService.EditAsync(taskId, version.Id, testCase);
        }

        return result;
    }

    public async Task<TaskDto> EditAsync(int taskId, EditVersionWithIdRequest dto)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var task = context.Tasks
            .Include(t => t.Themes)
            .Include(t => t.Versions)
            .ThenInclude(v => v.TestCases)
            .FirstOrDefault(t => t.Id == taskId) ?? throw new NotFoundException($"Task {taskId} not found");
        var version = task.Versions
            .FirstOrDefault(v => v.Id == dto.Id) ?? throw new NotFoundException($"Version {dto.Id} not found");

        version.Description = dto.Description;
        version.InputCondition = dto.InputCondition;
        version.OutputCondition = dto.OutputCondition;
        version.CodeTemplate = JsonConvert.SerializeObject(dto.CodeTemplate);

        await context.SaveChangesAsync();

        TaskDto result = _mapper.Map<TaskDto>(task);
        foreach (var testCase in dto.NewTestCases)
        {
            result = await _testCaseService.CreateAsync(taskId, version.Id, testCase);
        }

        foreach (var testCase in dto.EditedTestCases)
        {
            result = await _testCaseService.EditAsync(taskId, version.Id, testCase);
        }

        return result;
    }

    public async Task<VersionForTestDto[]> GetAllAsync(GetTaskVersionsRequestDto dto)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var tasks = context.Tasks
            .Include(t => t.Themes)
            .Include(t => t.Versions)
            .AsQueryable();
        if (dto.MinAvgTimeInMin != null)
        {
            tasks = tasks.Where(t => t.AvgTimeInMin >= dto.MinAvgTimeInMin);
        }
        if (dto.MaxAvgTimeInMin != null)
        {
            tasks = tasks.Where(t => t.AvgTimeInMin <= dto.MaxAvgTimeInMin);
        }
        if (dto.Complexity != null)
        {
            tasks = tasks.Where(t => t.Complexity == dto.Complexity);
        }
        if (dto.Type != null)
        {
            tasks = tasks.Where(t => t.Type == dto.Type);
        }
        var uploadedTasks = await tasks.ToListAsync();
        var resultTaskIds = new List<int>();
        if (dto.ThemeIds != null)
        {
            foreach (var task in uploadedTasks)
            {
                foreach (var theme in task.Themes)
                {
                    var themeDto = await _themeService.GetThemeWithParents(theme.Id);
                    if (IsRequestedTheme(themeDto, dto.ThemeIds))
                    {
                        resultTaskIds.Add(task.Id);
                        break;
                    }
                }
            }
        }
        else
        {
            resultTaskIds = uploadedTasks.Select(t => t.Id).ToList();
        }

        var versions = context.TaskVersions
            .Include(v => v.Task)
            .ThenInclude(t => t.Themes)
            .AsQueryable();

        if (dto.TaskVersionIds != null)
        {
            versions = versions.Where(v => dto.TaskVersionIds.Contains(v.Id));
        }
        versions = versions.Where(v => v.Task.IsEnabled && resultTaskIds.Contains(v.Task.Id));
        if (dto.ProgrammingLanguage != null)
        {
            versions = versions.Where(v => v.ProgrammingLanguage == (int)dto.ProgrammingLanguage);
        }

        var uploadedVersions = versions.ToList();

        return _mapper.Map<VersionForTestDto[]>(uploadedVersions);
    }

    public async Task<VersionForTestResultDto[]> GetAsync(GetTaskVersionsForTestResultRequestDto dto)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var versions = await context.TaskVersions
            .Include(v => v.Task)
            .Include(v => v.TestCases)
            .Where(v => dto.TaskVersionsIds.Contains(v.Id))
            .ToListAsync();

        return _mapper.Map<VersionForTestResultDto[]>(versions);
    }

    private bool IsRequestedTheme(ThemeDto dto, int[] allowedThemeIds)
    {
        while (dto != null)
        {
            if (allowedThemeIds.Contains(dto.Id))
            {
                return true;
            }

            dto = dto.SubThemes.FirstOrDefault();
        }

        return false;
    }
}
