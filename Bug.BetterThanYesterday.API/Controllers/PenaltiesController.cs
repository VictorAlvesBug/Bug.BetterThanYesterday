using Bug.BetterThanYesterday.Application.Payments.CreatePixCharge;
using Bug.BetterThanYesterday.Application.Penalties.ListPenaltiesByPlanId;
using Bug.BetterThanYesterday.Application.Penalties.ListPenaltiesByPlanMember;
using Bug.BetterThanYesterday.Application.Penalties.Models;
using Bug.BetterThanYesterday.Application.Penalties.PayPenalty;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Microsoft.AspNetCore.Mvc;

namespace Bug.BetterThanYesterday.API.Controllers;

[Route("api")]
[Route("testapi")]
[ApiController]
public class PenaltiesController(
	IUseCase<ListPenaltiesByPlanIdCommand> listPenaltiesByPlanIdUseCase,
	IUseCase<ListPenaltiesByPlanMemberCommand> listPenaltiesByPlanMemberUseCase,
	IUseCase<PayPenaltyCommand> payPenaltyUseCase,
	IUseCase<CreatePixChargeCommand> createPixChargeUseCase)
	: ControllerBase
{
	[HttpGet("Plans/{planId}/Penalties")]
	public async Task<IActionResult> ListByPlanId(Guid planId)
	{
		var command = new ListPenaltiesByPlanIdCommand(planId);
		var result = await listPenaltiesByPlanIdUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpGet("Plans/{planId}/Members/{userId}/Penalties")]
	[HttpGet("Plans/{planId}/Users/{userId}/Penalties")]
	public async Task<IActionResult> ListByPlanMember(Guid planId, Guid userId)
	{
		var command = new ListPenaltiesByPlanMemberCommand(planId, userId);
		var result = await listPenaltiesByPlanMemberUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpPost("Plans/{planId}/Penalties/{penaltyId}/Pay")]
	public async Task<IActionResult> PayPenalty(Guid planId, Guid penaltyId, [FromBody] PayPenaltyRequest request)
	{
		var command = new PayPenaltyCommand(planId, penaltyId, request.ConfirmedByUserId);
		var result = await payPenaltyUseCase.HandleAsync(command);

		if (result.IsSuccess())
		{
			var data = ((Result<PenaltyModel>)result).Data;
			return Ok(result);
		}

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpPost("Plans/{planId}/Penalties/{penaltyId}/Pix")]
	public async Task<IActionResult> CreatePixCharge(Guid planId, Guid penaltyId, [FromBody] CreatePixChargeRequest request)
	{
		var command = new CreatePixChargeCommand
		{
			PlanId = planId,
			PenaltyId = penaltyId,
			UserId = request.UserId
		};
		var result = await createPixChargeUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}
}

public sealed class PayPenaltyRequest
{
	public Guid ConfirmedByUserId { get; set; }
}

public sealed class CreatePixChargeRequest
{
	public Guid UserId { get; set; }
}
