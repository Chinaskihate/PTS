using PTS.Persistence.Models.Tests;
using System.ComponentModel.DataAnnotations;

namespace PTS.Persistence.Models.Results;
public class TestResult
{
    [Key]
    public int Id { get; set; }
    [Required]
    public Test Test { get; set; }
    [Required]
    public string StudentId { get; set; }
    public DateTime? SubmissionTime { get; set; }
    public List<TaskResult> TaskResults { get; set; }
}
