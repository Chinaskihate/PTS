﻿using System.ComponentModel.DataAnnotations;
using Task = PTS.Persistence.Models.Tasks.Task;

namespace PTS.Persistence.Models.Tasks;
public class Theme
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public bool IsBanned { get; set; } = false;
    public int? ParentId { get; set; }
    public Theme? Parent { get; set; }
    public List<Theme>? Children { get; set; }
    public List<Task> Tasks { get; set; } = [];
}
