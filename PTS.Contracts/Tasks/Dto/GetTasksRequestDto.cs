namespace PTS.Contracts.Tasks.Dto;
public class GetTasksRequestDto
{
    public ProgrammingLanguage? ProgrammingLanguage { get; set; }
    public int? MinAvgTimeInMin { get; set; }
    public int? MaxAvgTimeInMin { get; set; }
    public TaskComplexity? Complexity { get; set; }
    public TaskType? Type { get; set; }
    public int[]? ThemeIds { get; set; }
}
