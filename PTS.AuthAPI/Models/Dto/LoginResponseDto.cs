using PTS.Contracts.Users;

namespace PTS.AuthAPI.Models.Dto;

public class LoginResponseDto
{
    public UserDto User { get; set; }
    public string Token { get; set; }
    public string[] Roles { get; set; }
}
