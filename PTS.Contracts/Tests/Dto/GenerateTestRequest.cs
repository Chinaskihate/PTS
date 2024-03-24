namespace PTS.Contracts.Tests.Dto;

public class GenerateTestRequest
{
    public long? Time { get; set; }
    public string[] ProgrammingLanguage { get; set; } = [];
    public int[] ThemeIds { get; set; } = [];
    public long[] Difficult { get; set; } = [];
    public long TaskCount { get; set; }
}