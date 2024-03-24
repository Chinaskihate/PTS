using PTS.Contracts.PTSTestResults;

namespace PTS.Backend.Service.IService;
public interface ITestResultService
{
    Task<TestResultDto> StartTest(int testId);
}
