using Microsoft.AspNetCore.Identity;

namespace PTS.Persistence.Models.Users;
public class ApplicationUser : IdentityUser
{
    public string? TelegramId { get; set; }
    public bool IsBanned { get; set; } = false;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
