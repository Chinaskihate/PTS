using PTS.Persistence.Models.Users;

namespace PTS.AuthAPI.Service.IService;

public interface ITokenStorer
{
    void AddOrUpdateToken(ApplicationUser user, string token);
    bool RemoveToken(ApplicationUser user);
    bool CheckToken(string token);
}
