using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Microsoft.AspNetCore.Mvc;
using Bug.BetterThanYesterday.Application.PlanParticipants.AddUserToPlan;
using Bug.BetterThanYesterday.Application.PlanParticipants.GetPlanParticipantDetails;
using Bug.BetterThanYesterday.Application.PlanParticipants.GetPlanWithParticipantsByPlanId;
using Bug.BetterThanYesterday.Application.PlanParticipants.GetUserWithPlansByUserId;
using Bug.BetterThanYesterday.Application.PlanParticipants.BlockUserInThePlan;
using Bug.BetterThanYesterday.Application.PlanParticipants.UnblockUserInThePlan;
using Bug.BetterThanYesterday.Application.PlanParticipants.RemoveUserFromPlan;
using Bug.BetterThanYesterday.Application.PlanParticipants;

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

	[HttpPost("Plans/{planId}/Participants/{userId}")]
	[HttpPost("Users/{userId}/Plans/{planId}")]
	public async Task<IActionResult> AddUserToPlan(Guid planId, Guid userId)
	{
		var command = new AddUserToPlanCommand(planId, userId);
		var result = await addUserToPlanUseCase.HandleAsync(command);

		if (result.IsSuccess())
		{
			var data = ((Result<PlanParticipantDetailsModel>)result).Data;
			return Created($"Plans/{data.Plan.PlanId}/Participants/{data.Participant.UserId}", result);
		}

		if (result.IsRejected())
			return BadRequest(result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpDelete("Plans/{planId}/Participants/{userId}")]
	[HttpDelete("Users/{userId}/Plans/{planId}")]
	public async Task<IActionResult> RemoveUserFromPlan(Guid planId, Guid userId)
	{
		var command = new RemoveUserFromPlanCommand(planId, userId);
		var result = await removeUserFromPlanUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return NoContent();

		if (result.IsRejected())
			return BadRequest(result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpPost("Plans/{planId}/Participants/{userId}/Block")]
	[HttpPost("Users/{userId}/Plans/{planId}/Block")]
	public async Task<IActionResult> BlockUserInThePlan(Guid planId, Guid userId)
	{
		var command = new BlockUserInThePlanCommand(planId, userId);
		var result = await blockUserInThePlanUseCase.HandleAsync(command);

		if (result.IsSuccess())
		{
			var data = ((Result<PlanParticipantDetailsModel>)result).Data;
			return Created($"Plans/{data.Plan.PlanId}/Participants/{data.Participant.UserId}/Block", result);
		}

		if (result.IsRejected())
			return BadRequest(result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpDelete("Plans/{planId}/Participants/{userId}/Block")]
	[HttpDelete("Users/{userId}/Plans/{planId}/Block")]
	public async Task<IActionResult> UnblockUserInThePlan(Guid planId, Guid userId)
	{
		var command = new UnblockUserInThePlanCommand(planId, userId);
		var result = await unblockUserInThePlanUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return BadRequest(result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}
}
