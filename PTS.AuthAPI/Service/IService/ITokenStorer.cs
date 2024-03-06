namespace PTS.AuthAPI.Service.IService;

public interface ITokenStorer
{
    void AddOrUpdateToken(string userId, string token);
    bool RemoveToken(string userId);
    bool CheckToken(string token);
}
