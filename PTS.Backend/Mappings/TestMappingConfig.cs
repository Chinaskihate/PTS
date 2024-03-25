using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using PTS.Contracts.PTSTestResults;
using PTS.Contracts.Statistics;
using PTS.Contracts.Tasks.Dto;
using PTS.Contracts.Test;
using PTS.Contracts.TestCases.Dto;
using PTS.Contracts.Versions.Dto;
using PTS.Persistence.Models.Results;
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
            config.CreateMap<TestCaseDto, TestCaseForStudentDto>();
            config.CreateMap<TaskDto, IEnumerable<VersionForTestDto>>()
                .ConvertUsing<TaskToVersionForTestDtoConverter>();
            config.CreateMap<Test, TestDto>()
                .ForMember(dto => dto.TaskVersions, opt => opt.Ignore());
            config.CreateMap<TaskResult, TaskResultDto>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(t => t.Id));
            config.CreateMap<TestResult, TestResultDto>()
                .ForMember(dto => dto.Test, opt => opt.MapFrom(tr => tr.Test))
                .ForMember(dto => dto.IsCompleted, opt => opt.MapFrom(tr => tr.SubmissionTime != null))
                .ForMember(dto => dto.TaskResults, opt => opt.MapFrom(tr => tr.TaskResults));
            config.CreateMap<TestResult, TestStatisticsDto>()
                .ForMember(dto => dto.Test, opt => opt.MapFrom(tr => tr.Test));
        });

        return mappingConfig;
    }

    public class TaskToVersionForTestDtoConverter : ITypeConverter<TaskDto, IEnumerable<VersionForTestDto>>
    {
        public IEnumerable<VersionForTestDto> Convert(TaskDto source, IEnumerable<VersionForTestDto> destination, ResolutionContext context)
        {
            /*first mapp from Versions, then from Task*/
            foreach (var model in source.Versions.Select
                    (e => context.Mapper.Map<VersionForTestDto>(e)))
            {
                context.Mapper.Map(source, model);
                model.Id = source.Versions.Single().Id;
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
