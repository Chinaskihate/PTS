using Microsoft.AspNetCore.Identity;

namespace PTS.AuthAPI.Models;

public class ApplicationUser : IdentityUser
{
    public string TelegramId { get; set; }
}
