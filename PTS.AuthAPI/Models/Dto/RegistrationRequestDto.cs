namespace PTS.AuthAPI.Models.Dto;

public class RegistrationRequestDto
{
    public string Email { get; set; }
    public string TelegramId { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string? Role { get; set; }
}