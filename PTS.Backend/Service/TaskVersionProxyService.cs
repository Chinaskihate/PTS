using AutoMapper;
using Newtonsoft.Json;
using PTS.Backend.Service.IService;
using PTS.Backend.Utils;
using PTS.Contracts.PTSTestResults;
using PTS.Contracts.Tasks.Dto;
using PTS.Contracts.Versions.Dto;
using Serilog;
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
        Log.Warning($"Got {taskId} taskId {versionId} versionId");
        var serialized = JsonConvert.SerializeObject(response.Result);
        Log.Warning($"Serialized: {serialized}");
        var result = JsonConvert.DeserializeObject<TaskDto>(serialized);
        Log.Warning($"Deserialized: {result}");
        result.Versions = result.Versions.Where(v => v.Id == versionId).ToList();
        Log.Warning($"Versions: {result.Versions}");
        var mapped = _mapper.Map<IEnumerable<VersionForTestDto>>(result).ToList();
        Log.Warning($"Version: {result.Versions}");

        return mapped.First();
    }

    public async Task<List<VersionForTestResultDto>> GetFullAsync(GetTaskVersionsForTestResultRequestDto dto)
    {
        var response = await _baseService.SendAsync(new Contracts.Common.RequestDto
        {
            ApiType = Contracts.Common.ApiType.POST,
            Url = SD.TaskAPIBase + "/api/task/full",
            Data = dto
        });
        var result = new List<VersionForTestResultDto>();
        foreach (var item in (IEnumerable)response.Result)
        {
            var serialized = JsonConvert.SerializeObject(item);
            result.Add(JsonConvert.DeserializeObject<VersionForTestResultDto>(serialized));
        }

        return result;
    }
}
