using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.Habits.CreateHabit;
using Bug.BetterThanYesterday.Application.Habits.DeleteHabit;
using Bug.BetterThanYesterday.Application.Habits.GetHabitById;
using Bug.BetterThanYesterday.Application.Habits.ListAllHabits;
using Bug.BetterThanYesterday.Application.Habits.UpdateHabit;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Microsoft.AspNetCore.Mvc;
using IResult = Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure.IResult;

namespace Bug.BetterThanYesterday.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HabitsController(
	IUseCase<CreateHabitCommand, IResult> createHabitUseCase,
	IUseCase<ListAllHabitsCommand, IResult> listAllHabitsUseCase,
	IUseCase<GetHabitByIdCommand, IResult> getHabitByIdUseCase,
	IUseCase<UpdateHabitCommand, IResult> updateHabitUseCase,
	IUseCase<DeleteHabitCommand, IResult> deleteHabitUseCase)
	: ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> ListAll()
	{
		var command = new ListAllHabitsCommand();
		var result = await listAllHabitsUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return BadRequest(result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(Guid id)
	{
		var command = new GetHabitByIdCommand(id);
		var result = await getHabitByIdUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return BadRequest(result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateHabitCommand command)
	{
		var result = await createHabitUseCase.HandleAsync(command);

		if (result.IsSuccess())
		{
			var data = ((Result<HabitModel>)result).Data;
			return Created($"Habits/{data.Id}", result);
		}

		if (result.IsRejected())
			return BadRequest(result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpPut]
	public async Task<IActionResult> Update([FromBody] UpdateHabitCommand command)
	{
		var result = await updateHabitUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return BadRequest(result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		var command = new DeleteHabitCommand(id);
		var result = await deleteHabitUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return NoContent();

		if (result.IsRejected())
			return BadRequest(result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}
}
