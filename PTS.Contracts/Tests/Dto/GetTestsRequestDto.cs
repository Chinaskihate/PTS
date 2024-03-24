namespace PTS.Contracts.Tests.Dto;
public class GetTestsRequestDto
{
    public string? Name { get; set; }
    public int? TaskCount { get; set; }
    public int[]? ThemeIds { get; set; }
}
