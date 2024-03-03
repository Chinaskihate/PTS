using PTS.Contracts.Versions.Dto;

namespace PTS.Contracts.Tasks.Dto;
public class TaskDto
{
    public int Id { get; set; }
    public bool IsEnabled { get; set; }
    public int[] ThemeIds { get; set; } = null!;
    public TaskType TaskType { get; set; }
    public VersionDto[] Versions { get; set; } = null!;
}
