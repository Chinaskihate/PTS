namespace PTS.Contracts.Tests.Dto;
public class EditTestRequest
{
    public int? AllowedExecutionTimeInSec { get; set; }
    public string? Name { get; set; }
    public bool? IsEnabled { get; set; }
    public string? Description { get; set; }
}
