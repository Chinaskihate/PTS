using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PTS.Backend.Service.IService;
using PTS.Backend.Utils;
using PTS.Contracts.Tasks.Dto;
using PTS.Contracts.Versions.Dto;
using System.Collections;

namespace PTS.Backend.Service;
public class TaskVersionProxyService(
    IBaseService baseService,
    IMapper mapper) : ITaskVersionProxyService
{
    private readonly IBaseService _baseService = baseService;
    private readonly IMapper _mapper = mapper;

    public async Task<List<VersionForTestDto>> GetAllAsync()
    {
        var response = await _baseService.SendAsync(new Contracts.Common.RequestDto
        {
            ApiType = Contracts.Common.ApiType.GET,
            Url = SD.TaskAPIBase + "/api/task"
        });
        var result = new List<TaskDto>();
        foreach (var item in (IEnumerable)response.Result)
        {
            var serialized = JsonConvert.SerializeObject(item);
            result.Add(JsonConvert.DeserializeObject<TaskDto>(serialized));
        }

        var mapped = new List<VersionForTestDto>();
        foreach (var item in result)
        {
            mapped.AddRange(_mapper.Map<IEnumerable<VersionForTestDto>>(item));
        }

        return mapped;
    }

    public async Task<VersionForTestDto> GetAsync(int taskId, int versionId)
    {
        var response = await _baseService.SendAsync(new Contracts.Common.RequestDto
        {
            ApiType = Contracts.Common.ApiType.GET,
            Url = SD.TaskAPIBase + $"/api/task/{taskId}"
        });
        var serialized = JsonConvert.SerializeObject(response.Result);
        var result = JsonConvert.DeserializeObject<TaskDto>(serialized);
        result.Versions = result.Versions.Where(v => v.Id == versionId).ToList();
        var mapped = _mapper.Map<IEnumerable<VersionForTestDto>>(result);

        return mapped.First();
    }
}
