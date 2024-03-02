using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace PTS.Backend.Mappings;
public static class TaskMappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
        });

        return mappingConfig;
    }

    public static IServiceCollection AddTaskMapper(this IServiceCollection services)
    {
        IMapper mapper = RegisterMaps().CreateMapper();
        return services.AddSingleton(mapper);
    }
}
