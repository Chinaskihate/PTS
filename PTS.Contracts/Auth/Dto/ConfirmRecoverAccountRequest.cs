using System.ComponentModel.DataAnnotations;

namespace PTS.Contracts.Auth.Dto;
public class ConfirmRecoverAccountRequest
{
    [Required]
    public string UserId { get; set; }
    [Required]
    public string ConfirmationToken { get; set; }
    [Required]
    public string NewPassword { get; set; }
}
