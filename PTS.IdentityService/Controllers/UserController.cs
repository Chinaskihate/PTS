using Microsoft.AspNetCore.Mvc;

namespace PTS.IdentityService.Controllers;
[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }

    public class AuthResponse
    {
        public string Token { get; set; }
    }

    public class AuthenticateRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    [HttpPost("auth")]
    public AuthResponse Authenticate([FromBody] AuthenticateRequest authRequest)
    {
        return null;
    }

    [HttpPost("logout")]
    public void Logout()
    {

    }

    [HttpGet("{id}")]
    public GetUserResponse Get(long id)
    {
        return new GetUserResponse();
    }

    [HttpPost]
    public long Create([FromBody] CreateUserRequest createUserRequest)
    {
        return 0;
    }

    [HttpPost("{id}/ban")]
    public long BanUser(int id)
    {
        return id;
    }

    [HttpPost("{id}/unban")]
    public long UnbanUser(int id)
    {
        return id;
    }
}

public class CreateUserRequest
{
    public string Name { get; set; }

    public string Email { get; set; }
    
    // TODO: add registration by email code

    public string Password { get; set; }
}

public class GetUserResponse
{
    public long Id { get; set; }

    public string Name { get; set; }

    public bool IsBanned { get; set; }
}