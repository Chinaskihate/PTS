using PTS.Contracts.Users;
using PTS.Contracts.Users.Dto;

namespace PTS.AdminAPI.Services;

public interface IUserService
{
    Task<UserDto> EditUserAsync(string id, EditUserRequest dto);
    Task<List<UserDto>> GetUsersAsync();
    Task<UserDto> GetUserAsync(string id);
}
