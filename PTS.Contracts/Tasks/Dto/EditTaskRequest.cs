using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS.Contracts.Tasks.Dto;
public class EditTaskRequest
{
    [Required]
    public int[] ThemeIds { get; set; } = [];
    public bool IsEnabled { get; set; }
}
