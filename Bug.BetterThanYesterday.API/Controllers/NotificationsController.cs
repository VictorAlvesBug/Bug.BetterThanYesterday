using Bug.BetterThanYesterday.Application.Notifications.GenerateNotifications;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Microsoft.AspNetCore.Mvc;

namespace Bug.BetterThanYesterday.API.Controllers;

[Route("api")]
[Route("testapi")]
[ApiController]
public class NotificationsController(IUseCase<GenerateNotificationsCommand> generateNotificationsUseCase) : ControllerBase
{
	[HttpPost("Users/{userId}/Notifications")]
	public async Task<IActionResult> Generate(Guid userId)
	{
		var result = await generateNotificationsUseCase.HandleAsync(new GenerateNotificationsCommand(userId));

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}
}
