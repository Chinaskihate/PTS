using PTS.Contracts.Users;

namespace PTS.Contracts.Auth.Dto;

public class LoginResponseDto
{
    public UserDto User { get; set; }
    public string Token { get; set; }
    public string[] Roles { get; set; }
}
