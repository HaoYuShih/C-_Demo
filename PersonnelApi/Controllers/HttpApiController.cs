using Microsoft.AspNetCore.Mvc;
using PersonnelApi.Models;

namespace PersonnelApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HttpApiController : ControllerBase
{
    private readonly ILogger<HttpApiController> _logger;

    public HttpApiController(ILogger<HttpApiController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult SubmitPerson([FromBody] Person person)
    {
        _logger.LogInformation("HTTP API received person: {Name}, {Email}, {Age}", person.Name, person.Email, person.Age);

        // In a real application, you would save this to a database.
        // For this demo, we just return a success response.

        return Ok(new { Message = $"Received person {person.Name} via HTTP API." });
    }
}
