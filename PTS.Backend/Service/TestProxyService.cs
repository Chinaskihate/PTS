using Newtonsoft.Json;
using PTS.Backend.Service.IService;
using PTS.Backend.Utils;
using PTS.Contracts.Test;
using PTS.Contracts.Tests.Dto;

namespace PTS.Backend.Service;

public class TestProxyService(IBaseService baseService) : ITestProxyService
{
    private readonly IBaseService _baseService = baseService;

    public async Task<TestDto> Create(CreateTestRequest dto)
    {
        var response = await _baseService.SendAsync(new Contracts.Common.RequestDto
        {
            ApiType = Contracts.Common.ApiType.POST,
            Url = SD.TestManagerAPIBase + "/api/test",
            Data = dto
        });

        return JsonConvert.DeserializeObject<TestDto>(JsonConvert.SerializeObject(response!.Result))!;
    }
}