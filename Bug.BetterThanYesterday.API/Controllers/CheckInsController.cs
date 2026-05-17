using Bug.BetterThanYesterday.Application.CheckIns.AddCheckIn;
using Bug.BetterThanYesterday.Application.CheckIns.GetCheckInById;
using Bug.BetterThanYesterday.Application.CheckIns.ListCheckInsByFilter;
using Bug.BetterThanYesterday.Application.CheckIns;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Microsoft.AspNetCore.Mvc;
using Bug.BetterThanYesterday.Application.CheckIns.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bug.BetterThanYesterday.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CheckInsController(
	IUseCase<AddCheckInCommand> addCheckInUseCase,
	IUseCase<ListCheckInsByFilterCommand> listCheckInsByFilterUseCase,
	IUseCase<GetCheckInByIdCommand> getCheckInByIdUseCase)
	: ControllerBase
{
	[HttpGet("{checkInId}")]
	public async Task<IActionResult> GetById(Guid checkInId)
	{
		var command = new GetCheckInByIdCommand(checkInId);
		var result = await getCheckInByIdUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return BadRequest(result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpGet]
	public async Task<IActionResult> ListByFilter([FromQuery] ListCheckInsByFilterCommand command)
	{
		var result = await listCheckInsByFilterUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return BadRequest(result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpPost]
	public async Task<IActionResult> Add([FromBody] AddCheckInCommand command)
	{
		var result = await addCheckInUseCase.HandleAsync(command);

		if (result.IsSuccess())
		{
			var data = ((Result<CheckInModel>)result).Data;
			return Created($"CheckIns/{data.CheckInId}", result);
		}

		if (result.IsRejected())
			return BadRequest(result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}
}
