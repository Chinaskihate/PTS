using System.ComponentModel.DataAnnotations;

namespace PTS.Contracts.Auth.Dto;

public class RegistrationRequestDto
{
    [Required]
    public string Email { get; set; } = string.Empty;
    public string? TelegramId { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}