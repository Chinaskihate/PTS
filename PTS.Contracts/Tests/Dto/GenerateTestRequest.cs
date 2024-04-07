namespace PTS.Contracts.Tests.Dto;

public class GenerateTestRequest
{
    public int? Time { get; set; }
    public int[] ThemeIds { get; set; } = [];
    public long[] Difficult { get; set; } = [];
    public int TaskCount { get; set; }
}