using System.ComponentModel.DataAnnotations;

namespace PTS.Persistence.Models.Themes;
public class Theme
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public bool IsBanned { get; set; }
    public int? ParentId { get; set; }
    public Theme? Parent { get; set; }
    public List<Theme>? Children { get; set; }
}
