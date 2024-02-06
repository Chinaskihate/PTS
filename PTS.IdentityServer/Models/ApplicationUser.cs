using Microsoft.AspNetCore.Identity;

namespace PTS.IdentityServer.Models;

public class ApplicationUser : IdentityUser
{
    public string? TelegramId { get; set; }

    public int Role { get; set; }
}
