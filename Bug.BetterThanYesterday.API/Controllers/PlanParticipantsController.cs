using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Microsoft.AspNetCore.Mvc;
using Bug.BetterThanYesterday.Application.PlanParticipants.AddUserToPlan;
using Bug.BetterThanYesterday.Application.PlanParticipants.GetPlanParticipantDetails;
using Bug.BetterThanYesterday.Application.PlanParticipants.GetPlanWithParticipantsByPlanId;
using Bug.BetterThanYesterday.Application.PlanParticipants.GetUserWithPlansByUserId;
using Bug.BetterThanYesterday.Application.PlanParticipants.BlockUserInThePlan;
using Bug.BetterThanYesterday.Application.PlanParticipants.UnblockUserInThePlan;
using Bug.BetterThanYesterday.Application.PlanParticipants.RemoveUserFromPlan;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bug.BetterThanYesterday.API.Controllers;

[Route("api")]
[ApiController]
public class PlanPlarticipantsController(
	IUseCase<GetPlanParticipantDetailsCommand> getPlanParticipantDetailsUseCase,
	IUseCase<GetPlanWithParticipantsByPlanIdCommand> getPlanWithParticipantsByPlanIdUseCase,
	IUseCase<GetUserWithPlansByUserIdCommand> getUserWithPlansByUserIdUseCase,
	IUseCase<BlockUserInThePlanCommand> blockUserInThePlanUseCase,
	IUseCase<UnblockUserInThePlanCommand> unblockUserInThePlanUseCase,
	IUseCase<AddUserToPlanCommand> addUserToPlanUseCase,
	IUseCase<RemoveUserFromPlanCommand> removeUserFromPlanUseCase)
	: ControllerBase
{
	[HttpGet("Plans/{planId}/Participants")]
	public async Task<IActionResult> GetByPlanId(Guid planId)
	{
		var command = new GetPlanWithParticipantsByPlanIdCommand(planId);
		var result = await getPlanWithParticipantsByPlanIdUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return BadRequest(result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpGet("Plans/{planId}/Participants/{userId}")]
	[HttpGet("Users/{userId}/Plans/{planId}")]
	public async Task<IActionResult> GetByPlanIdAndUserId(Guid planId, Guid userId)
	{
		var command = new GetPlanParticipantDetailsCommand(planId, userId);
		var result = await getPlanParticipantDetailsUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return BadRequest(result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}
	
	[HttpGet("Users/{userId}/Plans")]
	public async Task<IActionResult> GetByUserId(Guid userId)
	{
		var command = new GetUserWithPlansByUserIdCommand(userId);
		var result = await getUserWithPlansByUserIdUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return BadRequest(result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	/*[HttpPost]
	public async Task<IActionResult> AddUserToPlan([FromBody] AddUserToPlanCommand command)
	{
		var result = await addUserToPlanUseCase.HandleAsync(command);

		if (result.IsSuccess())
		{
			var data = ((Result<PlanModel>)result).Data;
			return Created($"Plans/{data.PlanId}", result);
		}

		if (result.IsRejected())
			return BadRequest(result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}*/

	/*[HttpPut]
	public async Task<IActionResult> UpdateStatus([FromBody] UpdatePlanStatusCommand command)
	{
		var result = await updatePlanStatusUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return BadRequest(result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}*/

	/*[HttpDelete("{planId}")]
	public async Task<IActionResult> RemoveUserFromPlan(Guid planId)
	{
		var command = new CancelPlanCommand(planId);
		var result = await cancelPlanUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return NoContent();

		if (result.IsRejected())
			return BadRequest(result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}*/
}
