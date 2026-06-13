using Microsoft.AspNetCore.Mvc;

namespace Bug.BetterThanYesterday.API.Controllers;

[Route("api/[controller]")]
[Route("testapi/[controller]")]
[ApiController]
public class HealthCheckController()
	: ControllerBase
{
	[HttpGet()]
	public async Task<IActionResult> CheckHealth()
	{
		return StatusCode(StatusCodes.Status200OK, "API is healthy");
	}
}
