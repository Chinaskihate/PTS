namespace PTS.Contracts.Users;

public class UserDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string? TelegramId { get; set; }
    public bool IsBanned { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
