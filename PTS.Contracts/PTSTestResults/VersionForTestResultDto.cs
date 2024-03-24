using PTS.Contracts.Tasks;
using PTS.Contracts.TestCases.Dto;
using System.ComponentModel.DataAnnotations;

namespace PTS.Contracts.PTSTestResults;
public class VersionForTestResultDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int TaskId { get; set; }
    [Required]
    public ProgrammingLanguage ProgrammingLanguage { get; set; }
    [Required]
    public TaskType Type { get; set; }
    [Required]
    public TestCaseDto[] TestCases { get; set; }
}
