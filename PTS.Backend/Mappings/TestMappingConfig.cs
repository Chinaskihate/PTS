﻿using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using PTS.Contracts.Tasks.Dto;
using PTS.Contracts.Test;
using PTS.Contracts.Versions.Dto;
using PTS.Persistence.Models.Tests;

namespace PTS.Backend.Mappings;
public static class TestMappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<TaskDto, VersionForTestDto>()
                .ForMember(dto => dto.TaskId, opt => opt.MapFrom(t => t.Id))
                .ForMember(dto => dto.Themes, opt => opt.MapFrom(t => t.Themes));
            config.CreateMap<VersionDto, VersionForTestDto>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(t => t.Id));
            config.CreateMap<TaskDto, IEnumerable<VersionForTestDto>>()
                .ConvertUsing<TaskToVersionForTestDtoConverter>();

            config.CreateMap<Test, TestDto>()
                .ForMember(dto => dto.TaskVersions, opt => opt.Ignore());
        });

        return mappingConfig;
    }

    public class TaskToVersionForTestDtoConverter : ITypeConverter<TaskDto, IEnumerable<VersionForTestDto>>
    {
        public IEnumerable<VersionForTestDto> Convert(TaskDto source, IEnumerable<VersionForTestDto> destination, ResolutionContext context)
        {
            /*first mapp from People, then from Team*/
            foreach (var model in source.Versions.Select
                    (e => context.Mapper.Map<VersionForTestDto>(e)))
            {
                context.Mapper.Map(source, model);
                yield return model;
            }
        }
    }

    public static IServiceCollection AddTestMapper(this IServiceCollection services)
    {
        IMapper mapper = RegisterMaps().CreateMapper();
        return services.AddSingleton(mapper);
    }
}