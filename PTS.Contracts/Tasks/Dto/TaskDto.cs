using PTS.Contracts.Versions.Dto;

namespace PTS.Contracts.Tasks.Dto;
public class TaskDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsEnabled { get; set; }
    public TaskComplexity Complexity { get; set; }
    public int AvgTimeInMin { get; set; }
    public TaskType Type { get; set; }
    public int[] ThemeIds { get; set; } = null!;
    public VersionDto[] Versions { get; set; } = null!;
}
