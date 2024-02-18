using Microsoft.AspNetCore.Identity;

namespace PTS.Contracts.Users;
public class ApplicationUser : IdentityUser
{
    public string? TelegramId { get; set; }

    public UserRole Role { get; set; }
}
