using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PTS.Contracts.PTSTestResults;
using PTS.Contracts.Tasks;
using PTS.Contracts.Tasks.Dto;
using PTS.Contracts.TestCases.Dto;
using PTS.Contracts.Theme.Dto;
using PTS.Contracts.Versions.Dto;
using PTS.Persistence.Models.TestCases;
using PTS.Persistence.Models.Themes;
using PTS.Persistence.Models.Versions;
using Task = PTS.Persistence.Models.Tasks.Task;

namespace PTS.Backend.Mappings;
public static class TaskMappingConfig
{
    public static CodeTemplateDto? MapCodeTemplate(string codeTemplate)
    {
        if (string.IsNullOrWhiteSpace(codeTemplate))
        {
            return null;
        }

        return JsonConvert.DeserializeObject<CodeTemplateDto>(codeTemplate);
    }

    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<TestCase, TestCaseDto>();
            config.CreateMap<TestCase, TestCaseForStudentDto>();

            config.CreateMap<TaskVersion, VersionDto>()
                .ForMember(dto => dto.ProgrammingLanguage,
                    opt => opt.MapFrom(v => (ProgrammingLanguage)v.ProgrammingLanguage))
                .ForMember(dto => dto.TestCases,
                    opt => opt.MapFrom(v => v.TestCases))
                .ForMember(dto => dto.CodeTemplate,
                    opt => opt.MapFrom(v => MapCodeTemplate(v.CodeTemplate)));

            config.CreateMap<Theme, ThemeDto>()
                .ForMember(dto => dto.SubThemes, opt => opt.MapFrom(t => t.Children));
            config.CreateMap<Theme, ThemeForTestDto>();

            config.CreateMap<TaskVersion, VersionForTestDto>()
                .ForMember(dto => dto.Themes,
                    opt => opt.MapFrom(v => v.Task.Themes))
                .ForMember(dto => dto.ProgrammingLanguage,
                    opt => opt.MapFrom(v => v.ProgrammingLanguage))
                .ForMember(dto => dto.Type,
                    opt => opt.MapFrom(v => v.Task.Type))
                .ForMember(dto => dto.Name,
                    opt => opt.MapFrom(v => v.Task.Name))
                .ForMember(dto => dto.Complexity,
                    opt => opt.MapFrom(v => v.Task.Complexity))
                .ForMember(dto => dto.AvgTimeInMin,
                    opt => opt.MapFrom(v => v.Task.AvgTimeInMin))
                .ForMember(dto => dto.Id,
                    opt => opt.MapFrom(v => v.Id))
                .ForMember(dto => dto.CodeTemplate,
                    opt => opt.MapFrom(v => MapCodeTemplate(v.CodeTemplate)));

            config.CreateMap<Task, TaskDto>()
                .ForMember(dto => dto.Versions,
                    opt => opt.MapFrom(t => t.Versions))
                .ForMember(dto => dto.Themes,
                    opt => opt.MapFrom(t => t.Themes));

            config.CreateMap<TaskVersion, VersionForTestResultDto>()
                .ForMember(dto => dto.Id,
                    opt => opt.MapFrom(v => v.Id))
                .ForMember(dto => dto.ProgrammingLanguage,
                    opt => opt.MapFrom(v => v.ProgrammingLanguage))
                .ForMember(dto => dto.Type,
                    opt => opt.MapFrom(v => v.Task.Type))
                .ForMember(dto => dto.CodeTemplate,
                    opt => opt.MapFrom(v => MapCodeTemplate(v.CodeTemplate)));
        });

        return mappingConfig;
    }

    public static IServiceCollection AddTaskMapper(this IServiceCollection services)
    {
        IMapper mapper = RegisterMaps().CreateMapper();
        return services.AddSingleton(mapper);
    }
}
