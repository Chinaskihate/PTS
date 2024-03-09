namespace PTS.AuthAPI.Service.IService;

public interface ITokenStorer
{
    void AddOrUpdateToken(string userId, string token);
    void RemoveToken(string userId);
    bool CheckToken(string token);
}
