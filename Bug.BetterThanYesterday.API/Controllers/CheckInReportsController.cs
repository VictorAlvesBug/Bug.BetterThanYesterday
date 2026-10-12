using Bug.BetterThanYesterday.Application.Reports.CreateCheckInReport;
using Bug.BetterThanYesterday.Application.Reports.ListCheckInReports;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Microsoft.AspNetCore.Mvc;

namespace Bug.BetterThanYesterday.API.Controllers;

[Route("api/[controller]")]
[Route("testapi/[controller]")]
[ApiController]
public class CheckInReportsController(
	IUseCase<CreateCheckInReportCommand> createCheckInReportUseCase,
	IUseCase<ListCheckInReportsCommand> listCheckInReportsUseCase,
	IConfiguration configuration)
	: ControllerBase
{
	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateCheckInReportCommand command)
	{
		var result = await createCheckInReportUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpGet]
	public async Task<IActionResult> List([FromQuery] Guid planId, [FromQuery] Guid requesterId)
	{
		var adminUserId = configuration["Admin:UserId"];
		var isPlatformAdmin = Guid.TryParse(adminUserId, out var parsedAdminId) && parsedAdminId == requesterId;
		var result = await listCheckInReportsUseCase.HandleAsync(new ListCheckInReportsCommand
		{
			PlanId = planId,
			RequesterId = requesterId,
			IsPlatformAdmin = isPlatformAdmin
		});

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}
}
