using Microsoft.AspNetCore.Identity;

namespace PTS.Persistence.Models.Users;
public class ApplicationUser : IdentityUser
{
    public bool IsBanned { get; set; }

    public string? TelegramId { get; set; }

    public int Role { get; set; }
}
