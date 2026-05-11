using Bug.BetterThanYesterday.Application.AdminSettings;
using Bug.BetterThanYesterday.Application.AdminSettings.MoveInTime;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Microsoft.AspNetCore.Mvc;

namespace Bug.BetterThanYesterday.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminSettingsController(
	IUseCase<MoveInTimeCommand> moveInTimeUseCase)
	: ControllerBase
{
	[HttpPatch("MoveInTime")]
	public async Task<IActionResult> MoveInTime([FromBody] MoveInTimeCommand command)
	{
		var result = await moveInTimeUseCase.HandleAsync(command);

		if (result.IsSuccess())
		{
			return StatusCode(StatusCodes.Status200OK, result.GetMessage());
		}

		if (result.IsRejected())
			return BadRequest(result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}
}
