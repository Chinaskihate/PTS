using System.ComponentModel.DataAnnotations;

namespace PTS.Contracts.Users;
public class RevokeTokenDto
{
    [Required]
    public string? UserName { get; set; }
}
