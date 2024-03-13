using System.ComponentModel.DataAnnotations;

namespace PTS.Contracts.Test;
public class TestDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public bool IsEnabled { get; set; }
}
