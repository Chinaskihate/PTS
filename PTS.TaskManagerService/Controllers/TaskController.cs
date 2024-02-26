using Microsoft.AspNetCore.Mvc;

namespace PTS.TaskManagerService.Controllers;
[ApiController]
[Route("[controller]")]
public class TaskController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<TaskController> _logger;

    public TaskController(ILogger<TaskController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public long CreateTask([FromBody] CreateTaskRequest request)
    {
        return 0;
    }

    [HttpGet("{id}")]
    public GetTaskResponse GetTask(long id)
    {
        return null;
    }

    [HttpPost("{id}")]
    public GetTaskResponse ChangeTask(int taskId, [FromBody] ChangeTaskRequest request)
    {
        return null;
    }

    [HttpPost("{id}/version")]
    public long CreateVersion(int taskId, [FromBody] CreateVersionRequest request)
    {
        return 0;
    }

    [HttpGet("{taskId}/version/{versionId}")]
    public GetVersionResponse GetVersion(int taskId, int versionId)
    {
        return null;
    }

    [HttpPost("{taskId}/version/{versionId}")]
    public GetVersionResponse ChangeVersion(int taskId, int versionId, [FromBody] ChangeVersionRequest request)
    {
        return null;
    }
}

public class CreateTaskRequest
{
    public long[] ThemeIds { get; set; }

    public ProgrammingLanguage ProgrammingLanguage { get; set; }

    public bool IsEnabled { get; set; }

    public TaskType Type { get; set; }
}

public class ChangeTaskRequest
{
    public long[] ThemeIds { get; set; }

    public bool IsEnabled { get; set; }
}

public class ChangeVersionRequest
{
    public ProgrammingLanguage ProgrammingLanguage { get; set; }

    public string SpecificDescription { get; set; }

    public bool UseSubstitutions { get; set; }
}

public class CreateVersionRequest
{
    public long TaskId { get; set; }

    public ProgrammingLanguage ProgrammingLanguage { get; set; }

    public string Description { get; set; }
}

public class EditVersionRequest
{
    public string Description { get; set; }
}

public class CreateTestCase
{
    public string? Input { get; set; }
    public string Output { get; set; }
}

public class EditTestCase
{
    public string? Input { get; set; }
    public string Output { get; set; }
}

public class GetVersionResponse
{
    public long VersionId { get; set; }

    public long TaskId { get; set; }

    public ProgrammingLanguage ProgrammingLanguage { get; set; }

    // todo:
    public long CreatedBy { get; set; }

    public long LastEditedBy { get; set; }

    public string SpecificDescription { get; set; }

    public bool UseSubstitutions { get; set; }
}

public enum ProgrammingLanguage { }

public enum TaskType { }


public class GetTaskResponse
{
    public long Id { get; set; }

    public long[] ThemeIds { get; set; }

    public bool IsEnabled { get; set; }

    public TaskType Type { get; set; }

    public VersionDto[] Versions { get; set; }
}

public class VersionDto
{
    public long Id { get; set; }

    public long TaskId { get; set; }

    public ProgrammingLanguage ProgrammingLanguage { get; set; }

    public string Description { get; set; }
}