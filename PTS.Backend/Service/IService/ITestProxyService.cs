using PTS.Contracts.Test;
using PTS.Contracts.Tests.Dto;

namespace PTS.Backend.Service.IService;

public interface ITestProxyService
{
    Task<TestDto> Create(CreateTestRequest dto);
}