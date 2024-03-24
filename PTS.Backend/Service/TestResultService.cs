using Microsoft.EntityFrameworkCore;
using PTS.Backend.Service.IService;
using PTS.Contracts.PTSTestResults;
using PTS.Persistence.DbContexts;

namespace PTS.Backend.Service;
public class TestResultService(
    IDbContextFactory<TestDbContext> dbContextFactory,
    ITokenProvider tokenProvider) : ITestResultService
{
    private readonly IDbContextFactory<TestDbContext> _dbContextFactory = dbContextFactory;
    private readonly ITokenProvider _tokenProvider = tokenProvider;

    public Task<TestResultDto> StartTest(int testId)
    {
        throw new NotImplementedException();
    }
}
