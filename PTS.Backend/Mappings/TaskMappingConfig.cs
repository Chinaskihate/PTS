﻿using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using PTS.Backend.Service.IService;
using PTS.Contracts.Tasks;
using PTS.Contracts.Tasks.Dto;
using PTS.Contracts.Versions.Dto;
using PTS.Persistence.Models.Versions;
using Task = PTS.Persistence.Models.Tasks.Task;

namespace PTS.Backend.Mappings;
public static class TaskMappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<TaskVersion, VersionDto>()
                .ForMember(dto => dto.ProgrammingLanguage,
                    opt => opt.MapFrom(v => (ProgrammingLanguage)v.ProgrammingLanguage));
            config.CreateMap<Task, TaskDto>()
                .ForMember(dto => dto.Versions,
                    opt => opt.MapFrom(t => t.Versions));
        });

        return mappingConfig;
    }

    public static IServiceCollection AddTaskMapper(this IServiceCollection services)
    {
        IMapper mapper = RegisterMaps().CreateMapper();
        return services.AddSingleton(mapper);
    }
}
