using PTS.Contracts.Theme.Dto;

namespace PTS.Backend.Service.IService;
public interface IThemeService
{
    Task<long> CreateThemeAsync(CreateThemeRequest request);

    Task<ThemeDto> EditThemeAsync(int themeId, EditThemeRequest request);

    Task<ThemeDto> GetThemes(bool availableOnly = false);
}
