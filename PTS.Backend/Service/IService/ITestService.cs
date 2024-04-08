using PTS.Contracts.Test;
using PTS.Contracts.Tests.Dto;

namespace PTS.Backend.Service.IService;
public interface ITestService
{
    Task<TestDto> Create(CreateTestRequest dto, string userId);
    Task<TestDto> Get(int id);
    Task<List<TestDto>> GetAllAsync(GetTestsRequestDto dto);
}
