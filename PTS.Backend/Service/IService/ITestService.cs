using PTS.Contracts.Test;
using PTS.Contracts.Tests.Dto;

namespace PTS.Backend.Service.IService;
public interface ITestService
{
    Task<TestDto> CreateAsync(CreateTestRequest dto, string userId);
    Task<TestDto> EditAsync(EditTestRequest dto, int id);
    Task<TestDto> Get(int id);
    Task<List<TestDto>> GetAllAsync(GetTestsRequestDto dto, string userId);
}
