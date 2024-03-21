using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PTS.Backend.Exceptions.Common;
using PTS.Backend.Service.IService;
using PTS.Contracts.Constants;
using PTS.Contracts.Theme.Dto;
using PTS.Persistence.DbContexts;
using PTS.Persistence.Models.Themes;

namespace PTS.Backend.Service;
public class ThemeService(
    IDbContextFactory<TaskDbContext> dbFactory,
    IMapper mapper) : IThemeService
{
    private readonly IDbContextFactory<TaskDbContext> _dbFactory = dbFactory;
    private readonly IMapper _mapper = mapper;

    public async Task<long> CreateThemeAsync(CreateThemeRequest request)
    {
        using var context = _dbFactory.CreateDbContext();
        var rootTheme = await context.Themes.FirstOrDefaultAsync(t => t.Id == request.RootId);
        var newTheme = new Theme
        {
            ParentId = rootTheme.Id,
            Name = request.Name
        };
        var createdTheme = await context.Themes.AddAsync(newTheme);
        await context.SaveChangesAsync();

        return createdTheme.Entity.Id;
    }

    public async Task<ThemeDto> EditThemeAsync(int themeId, EditThemeRequest request)
    {
        using var context = _dbFactory.CreateDbContext();
        var theme = await context.Themes.FirstOrDefaultAsync(t => t.Id == themeId);
        theme.Name = request.Name;
        theme.IsBanned = request.IsBanned ?? false;
        context.Themes.Update(theme);
        await context.SaveChangesAsync();

        return await GetThemes();
    }

    public async Task<ThemeDto> GetThemes(bool availableOnly = false)
    {
        using var context = _dbFactory.CreateDbContext();
        return await GetThemes(availableOnly, context);
    }

    public async Task<ThemeDto> GetThemeWithParents(int childThemeId, bool availableOnly = false)
    {
        using var context = _dbFactory.CreateDbContext();

        Theme root = await context.Themes
            .Include(t => t.Parent)
            .FirstAsync(t => t.Id == childThemeId && (!availableOnly || !t.IsBanned));

        while(root?.Parent != null)
        {
            root = await context.Themes
                .Include(t => t.Parent)
                .FirstOrDefaultAsync(t => t.Id == root.ParentId && (!availableOnly || !t.IsBanned));
        }        

        return _mapper.Map<ThemeDto>(root ?? throw new NotFoundException($"No available theme with {childThemeId}"));
    }

    private async Task<ThemeDto> GetThemes(bool availableOnly, TaskDbContext context)
    {
        Theme root = await context.Themes
            .FirstAsync(t => t.Id == Constants.GlobalRootThemeId);
        var uploadedRoot = await GetThemeWithAllChildren(root, availableOnly, context);
        return uploadedRoot;
    }

    private async Task<ThemeDto> GetThemeWithAllChildren(Theme currentNode, bool availableOnly, TaskDbContext context)
    {
        var uploadedNode = await context.Themes
            .Include(t => t.Children)
            .FirstAsync(t => t.Id == currentNode.Id);
        var result = _mapper.Map<ThemeDto>(uploadedNode);
        if (uploadedNode.Children != null && uploadedNode.Children.Count > 0)
        {
            result.SubThemes = new List<ThemeDto>();
            foreach (var node in uploadedNode.Children)
            {
                if (!availableOnly || !node.IsBanned)
                {
                    result.SubThemes.Add(await GetThemeWithAllChildren(node, availableOnly, context));
                }
            }
        }

        if (availableOnly)
        {
            result.SubThemes = result.SubThemes?.Where(result => !result.IsBanned).ToList();
        }
        return result;
    }
}
