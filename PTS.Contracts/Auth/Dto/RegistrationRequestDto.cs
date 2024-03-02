namespace PTS.Contracts.Auth.Dto;

public class RegistrationRequestDto
{
    public string Email { get; set; }
    public string TelegramId { get; set; }
    public string Password { get; set; }
}