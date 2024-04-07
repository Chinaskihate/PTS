using PTS.Contracts.Tests.Dto;

namespace PTS.Backend.Service.IService;

public interface ITestGeneratorService
{
    Task<long> GenerateTest(GenerateTestRequest dto);
}