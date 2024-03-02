using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using PTS.Contracts.Users;
using PTS.Persistence.Models.Users;

namespace PTS.Backend.Mappings;
public static class UserMappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<UserDto, ApplicationUser>();
            config.CreateMap<ApplicationUser, UserDto>();
        });

        return mappingConfig;
    }

    public static IServiceCollection AddUserMapper(this IServiceCollection services)
    {
        IMapper mapper = RegisterMaps().CreateMapper();
        return services.AddSingleton(mapper);
    }
}
