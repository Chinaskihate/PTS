using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using PTS.Contracts.Theme.Dto;
using PTS.Persistence.Models.Tasks;

namespace PTS.Backend.Mappings;
public static class ThemeMappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<ThemeDto, Theme>();
            config.CreateMap<Theme, ThemeDto>()
                .ForMember(dto => dto.SubThemes, opt => opt.MapFrom(t => t.Children));
        });

        return mappingConfig;
    }

    public static IServiceCollection AddThemeMapper(this IServiceCollection services)
    {
        IMapper mapper = RegisterMaps().CreateMapper();
        return services.AddSingleton(mapper);
    }
}
