using PTS.Backend.Service.IService;
using PTS.Contracts.Versions.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS.Backend.Service;
public class TaskVersionProxyService(
    IHttpClientFactory httpClientFactory,
    ITokenProvider tokenProvider) : BaseService(httpClientFactory, tokenProvider), ITaskVersionProxyService
{
    public Task<List<VersionForTestDto>> GetAllAsync(int versionId)
    {
        throw new NotImplementedException();
    }

    public Task<VersionForTestDto> GetAsync(int versionId)
    {
        throw new NotImplementedException();
    }
}
