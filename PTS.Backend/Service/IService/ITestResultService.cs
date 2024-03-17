using PTS.Contracts.TestResults;

namespace PTS.Backend.Service.IService;
public interface ITestResultService
{
    Task<TestResultDto> StartTest(int testId);
}
