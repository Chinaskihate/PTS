﻿using PTS.Contracts.PTSTestResults;

namespace PTS.Backend.Service.IService;
public interface ITestExecutionService
{
    Task<TestResultDto> StartTestAsync(StartTestDto dto, string userId);
    Task<List<TestResultDto>> GetUserTestsAsync(string userId);
    Task<TestResultDto> SubmitTaskAsync(SubmitTaskDto dto, string userId);
    Task<TestResultDto> GetTestStatusAsync(int testResultId, string userId);
}
