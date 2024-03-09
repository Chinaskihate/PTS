using PTS.AuthAPI.Service.IService;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;

namespace PTS.AuthAPI.Service;

public class TokenStorer : ITokenStorer
{
    private readonly ConcurrentDictionary<string, string> _tokens = new ConcurrentDictionary<string, string>();

    public void AddOrUpdateToken(string userId, string token)
    {
        _tokens.AddOrUpdate(userId, token, (key, oldValue) => token);
    }

    public bool CheckToken(string token)
    {
        if(string.IsNullOrWhiteSpace(token)) return false;

        var handler = new JwtSecurityTokenHandler();

        var jwt = handler.ReadJwtToken(token);

        var claim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

        if (string.IsNullOrWhiteSpace(claim?.Value))
        {
            return false;
        }

        return _tokens.ContainsKey(claim.Value);
    }

    public void RemoveToken(string userId)
    {
        _tokens.Remove(userId, out _);
    }
}
