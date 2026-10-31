using ApplyPilot.Application.DTOs.Common;
using Microsoft.AspNetCore.Mvc;

namespace ApplyPilot.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Check()
    {
        return Ok(ApiResponse<object>.Ok(new
        {
            Status = "Healthy",
            Service = "ApplyPilot.Api",
            Version = "1.0.0",
            Timestamp = DateTime.UtcNow
        }, "ApplyPilot AI Backend API is online."));
    }
}

