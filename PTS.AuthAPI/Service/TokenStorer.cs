using PTS.AuthAPI.Service.IService;
using PTS.Persistence.Models.Users;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;

namespace PTS.AuthAPI.Service;

public class TokenStorer : ITokenStorer
{
    private readonly ConcurrentDictionary<string, string> _tokens = new ConcurrentDictionary<string, string>();

    public void AddOrUpdateToken(ApplicationUser user, string token)
    {
        _tokens.AddOrUpdate(user.Id, token, (key, oldValue) => token);
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

    public bool RemoveToken(ApplicationUser user)
    {
        return _tokens.Remove(user.Id, out _);
    }
}
