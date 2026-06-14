using Bug.BetterThanYesterday.Application.Plans.CreatePlan;
using Bug.BetterThanYesterday.Application.Plans.CancelPlan;
using Bug.BetterThanYesterday.Application.Plans.GetPlanById;
using Bug.BetterThanYesterday.Application.Plans.ListPlansByFilter;
using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Microsoft.AspNetCore.Mvc;
using Bug.BetterThanYesterday.Application.Plans.ListPlansByHabitId;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bug.BetterThanYesterday.API.Controllers;

[Route("api/[controller]")]
[Route("testapi/[controller]")]
[ApiController]
public class PlansController(
	IUseCase<CreatePlanCommand> createPlanUseCase,
	IUseCase<ListPlansByFilterCommand> listPlansByFilterUseCase,
	IUseCase<GetPlanByIdCommand> getPlanByIdUseCase/*,
	IUseCase<ListPlansByHabitIdCommand> listPlansByHabitIdUseCase,
	IUseCase<CancelPlanCommand> cancelPlanUseCase*/)
	: ControllerBase
{
	[HttpGet("{planId}")]
	public async Task<IActionResult> GetById(Guid planId)
	{
		var command = new GetPlanByIdCommand(planId);
		var result = await getPlanByIdUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpGet]
	public async Task<IActionResult> ListByFilter([FromQuery] ListPlansByFilterCommand command)
	{
		var result = await listPlansByFilterUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreatePlanCommand command)
	{
		var result = await createPlanUseCase.HandleAsync(command);

		if (result.IsSuccess())
		{
			var data = ((Result<PlanModel>)result).Data;
			return Created($"Plans/{data.Id}", result);
		}

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	/*[HttpDelete("{planId}")]
	public async Task<IActionResult> Cancel(Guid planId)
	{
		var command = new CancelPlanCommand(planId);
		var result = await cancelPlanUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return NoContent();

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}*/

	/*[HttpGet]
	private async Task<IActionResult> ListByHabitId(Guid habitId)
	{
		var command = new ListPlansByHabitIdCommand(habitId);
		var result = await listPlansByHabitIdUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}*/
}
