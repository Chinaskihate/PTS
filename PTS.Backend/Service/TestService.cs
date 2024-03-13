using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PTS.Backend.Service.IService;
using PTS.Contracts.Test;
using PTS.Persistence.DbContexts;

namespace PTS.Backend.Service;
public class TestService(
    IDbContextFactory<TestDbContext> dbContextFactory,
    IMapper mapper) : ITestService
{
    private readonly IDbContextFactory<TestDbContext> _dbContextFactory = dbContextFactory;
    private readonly IMapper _mapper = mapper;

    public Task<TestDto> Get(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<TestDto>> GetAll()
    {
        throw new NotImplementedException();
    }
}
