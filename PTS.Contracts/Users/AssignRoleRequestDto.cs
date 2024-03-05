using System.ComponentModel.DataAnnotations;

namespace PTS.Contracts.Users;
public class AssignRoleRequestDto
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Role { get; set; }
}
