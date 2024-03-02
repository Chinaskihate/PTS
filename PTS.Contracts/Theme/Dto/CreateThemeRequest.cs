using System.ComponentModel.DataAnnotations;

namespace PTS.Contracts.Theme.Dto;
public class CreateThemeRequest
{
    [Required]
    public string Name { get; set; }

    [Required]
    public long RootId { get; set; }
}