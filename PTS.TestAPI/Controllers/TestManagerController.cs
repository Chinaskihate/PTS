using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PTS.TestAPI.Controllers;

[ApiController]
[Route("api/task")]
[Authorize]
public class TestManagerController
{
}
