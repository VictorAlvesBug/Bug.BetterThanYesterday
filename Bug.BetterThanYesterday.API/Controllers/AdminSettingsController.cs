using Bug.BetterThanYesterday.Application.AdminSettings;
using Bug.BetterThanYesterday.Application.AdminSettings.DeleteMockData;
using Bug.BetterThanYesterday.Application.AdminSettings.MoveInTime;
using Bug.BetterThanYesterday.Application.AdminSettings.PersistMockData;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Microsoft.AspNetCore.Mvc;

namespace Bug.BetterThanYesterday.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminSettingsController(
	IUseCase<MoveInTimeCommand> moveInTimeUseCase,
	IUseCase<PersistMockDataCommand> persistMockDataUseCase,
	IUseCase<DeleteMockDataCommand> deleteMockDataUseCase)
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
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpPut("PersistMockData")]
	public async Task<IActionResult> PersistMockData()
	{
		var result = await persistMockDataUseCase.HandleAsync(new PersistMockDataCommand());

		if (result.IsSuccess())
		{
			return StatusCode(StatusCodes.Status200OK, result.GetMessage());
		}

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpDelete("DeleteMockData")]
	public async Task<IActionResult> DeleteMockData()
	{
		var result = await deleteMockDataUseCase.HandleAsync(new DeleteMockDataCommand());

		if (result.IsSuccess())
		{
			return StatusCode(StatusCodes.Status200OK, result.GetMessage());
		}

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}
}
