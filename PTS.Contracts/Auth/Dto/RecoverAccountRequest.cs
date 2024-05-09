using System.ComponentModel.DataAnnotations;

namespace PTS.Contracts.Auth.Dto;
public class RecoverAccountRequest
{
    [Required]
    public string Email { get; set; }
}
