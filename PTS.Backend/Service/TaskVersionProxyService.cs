using AutoMapper;
using Newtonsoft.Json;
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

    public async Task<List<VersionForTestDto>> GetAllAsync(GetTaskVersionsRequestDto dto)
    {
        var response = await _baseService.SendAsync(new Contracts.Common.RequestDto
        {
            ApiType = Contracts.Common.ApiType.POST,
            Url = SD.TaskAPIBase + "/api/task/splitted",
            Data = dto
        });
        var result = new List<VersionForTestDto>();
        foreach (var item in (IEnumerable)response.Result)
        {
            var serialized = JsonConvert.SerializeObject(item);
            result.Add(JsonConvert.DeserializeObject<VersionForTestDto>(serialized));
        }

        return result;
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
