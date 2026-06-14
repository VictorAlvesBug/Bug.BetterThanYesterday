using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Microsoft.AspNetCore.Mvc;
using Bug.BetterThanYesterday.Application.PlanMembers.AddUserToPlan;
using Bug.BetterThanYesterday.Application.PlanMembers.GetPlanMemberDetails;
using Bug.BetterThanYesterday.Application.PlanMembers.GetPlanWithMembersByPlanId;
using Bug.BetterThanYesterday.Application.PlanMembers.GetUserWithPlansByUserId;
using Bug.BetterThanYesterday.Application.PlanMembers.BlockUserInThePlan;
using Bug.BetterThanYesterday.Application.PlanMembers.UnblockUserInThePlan;
using Bug.BetterThanYesterday.Application.PlanMembers.RemoveUserFromPlan;
using Bug.BetterThanYesterday.Application.PlanMembers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bug.BetterThanYesterday.API.Controllers;

[Route("api")]
[Route("testapi")]
[ApiController]
public class PlanPlarticipantsController(
	IUseCase<GetPlanMemberDetailsCommand> getPlanMemberDetailsUseCase,
	IUseCase<GetPlanWithMembersByPlanIdCommand> getPlanWithMembersByPlanIdUseCase,
	IUseCase<GetUserWithPlansByUserIdCommand> getUserWithPlansByUserIdUseCase,
	IUseCase<BlockUserInThePlanCommand> blockUserInThePlanUseCase,
	IUseCase<UnblockUserInThePlanCommand> unblockUserInThePlanUseCase,
	IUseCase<AddUserToPlanCommand> addUserToPlanUseCase,
	IUseCase<RemoveUserFromPlanCommand> removeUserFromPlanUseCase)
	: ControllerBase
{
	[HttpGet("Plans/{planId}/Members")]
	[HttpGet("Plans/{planId}/Users")]
	public async Task<IActionResult> GetByPlanId(Guid planId)
	{
		var command = new GetPlanWithMembersByPlanIdCommand(planId);
		var result = await getPlanWithMembersByPlanIdUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

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
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpGet("Plans/{planId}/Members/{userId}")]
	[HttpGet("Plans/{planId}/Users/{userId}")]
	//[HttpGet("Users/{userId}/Plans/{planId}")]
	public async Task<IActionResult> GetByPlanIdAndUserId(Guid planId, Guid userId)
	{
		var command = new GetPlanMemberDetailsCommand(planId, userId);
		var result = await getPlanMemberDetailsUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpPost("Plans/{planId}/Members/{userId}")]
	[HttpPost("Plans/{planId}/Users/{userId}")]
	//[HttpPost("Users/{userId}/Plans/{planId}")]
	public async Task<IActionResult> AddUserToPlan(Guid planId, Guid userId)
	{
		var command = new AddUserToPlanCommand(planId, userId);
		var result = await addUserToPlanUseCase.HandleAsync(command);

		if (result.IsSuccess())
		{
			var data = ((Result<PlanMemberDetailsModel>)result).Data;
			return Created($"Plans/{data.Plan.Id}/Members/{data.User.Id}", result);
		}

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpDelete("Plans/{planId}/Members/{userId}")]
	[HttpDelete("Plans/{planId}/Users/{userId}")]
	//[HttpDelete("Users/{userId}/Plans/{planId}")]
	public async Task<IActionResult> RemoveUserFromPlan(Guid planId, Guid userId)
	{
		var command = new RemoveUserFromPlanCommand(planId, userId);
		var result = await removeUserFromPlanUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return NoContent();

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpPost("Plans/{planId}/Members/{userId}/Block")]
	[HttpPost("Plans/{planId}/Users/{userId}/Block")]
	//[HttpPost("Users/{userId}/Plans/{planId}/Block")]
	public async Task<IActionResult> BlockUserInThePlan(Guid planId, Guid userId)
	{
		var command = new BlockUserInThePlanCommand(planId, userId);
		var result = await blockUserInThePlanUseCase.HandleAsync(command);

		if (result.IsSuccess())
		{
			var data = ((Result<PlanMemberDetailsModel>)result).Data;
			return Created($"Plans/{data.Plan.Id}/Members/{data.User.Id}/Block", result);
		}

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpDelete("Plans/{planId}/Members/{userId}/Block")]
	[HttpDelete("Plans/{planId}/Users/{userId}/Block")]
	//[HttpDelete("Users/{userId}/Plans/{planId}/Block")]
	public async Task<IActionResult> UnblockUserInThePlan(Guid planId, Guid userId)
	{
		var command = new UnblockUserInThePlanCommand(planId, userId);
		var result = await unblockUserInThePlanUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}
}
