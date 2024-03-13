using PTS.Contracts.Test;

namespace PTS.Backend.Service.IService;
public interface ITestService
{
    Task<TestDto> Get(int id);
    Task<List<TestDto>> GetAll();
}
