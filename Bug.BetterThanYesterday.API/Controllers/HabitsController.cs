using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.Habits.CreateHabit;
using Bug.BetterThanYesterday.Application.Habits.DeleteHabit;
using Bug.BetterThanYesterday.Application.Habits.GetHabitById;
using Bug.BetterThanYesterday.Application.Habits.ListHabitsByFilter;
using Bug.BetterThanYesterday.Application.Habits.UpdateHabit;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Microsoft.AspNetCore.Mvc;

namespace Bug.BetterThanYesterday.API.Controllers;

[Route("api/[controller]")]
[Route("testapi/[controller]")]
[ApiController]
public class HabitsController(
	IUseCase<GetHabitByIdCommand> getHabitByIdUseCase,
	IUseCase<ListHabitsByFilterCommand> listHabitsByFilterUseCase,
	IUseCase<CreateHabitCommand> createHabitUseCase/*,
	IUseCase<UpdateHabitCommand> updateHabitUseCase,
	IUseCase<DeleteHabitCommand> deleteHabitUseCase*/)
	: ControllerBase
{

	[HttpGet("{habitId}")]
	public async Task<IActionResult> GetById(Guid habitId)
	{
		var command = new GetHabitByIdCommand(habitId);
		var result = await getHabitByIdUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}
	[HttpGet]
	public async Task<IActionResult> ListByFilter([FromQuery] ListHabitsByFilterCommand command)
	{
		var result = await listHabitsByFilterUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateHabitCommand command)
	{
		var result = await createHabitUseCase.HandleAsync(command);

		if (result.IsSuccess())
		{
			var data = ((Result<HabitModel>)result).Data;
			return Created($"Habits/{data.HabitId}", result);
		}

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	/*[HttpPut]
	public async Task<IActionResult> Update([FromBody] UpdateHabitCommand command)
	{
		var result = await updateHabitUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}*/

	/*[HttpDelete("{habitId}")]
	public async Task<IActionResult> Delete(Guid habitId)
	{
		var command = new DeleteHabitCommand(habitId);
		var result = await deleteHabitUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}*/
}
