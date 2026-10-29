using Bug.BetterThanYesterday.Application.Payments.CreatePixCharge;
using Bug.BetterThanYesterday.Application.Penalties.ListPenaltiesByChallengeId;
using Bug.BetterThanYesterday.Application.Penalties.ListPenaltiesByChallengeMember;
using Bug.BetterThanYesterday.Application.Penalties.Models;
using Bug.BetterThanYesterday.Application.Penalties.PayPenalty;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Microsoft.AspNetCore.Mvc;

namespace Bug.BetterThanYesterday.API.Controllers;

[Route("api")]
[Route("testapi")]
[ApiController]
public class PenaltiesController(
	IUseCase<ListPenaltiesByChallengeIdCommand> listPenaltiesByChallengeIdUseCase,
	IUseCase<ListPenaltiesByChallengeMemberCommand> listPenaltiesByChallengeMemberUseCase,
	IUseCase<PayPenaltyCommand> payPenaltyUseCase,
	IUseCase<CreatePixChargeCommand> createPixChargeUseCase)
	: ControllerBase
{
	[HttpGet("Challenges/{challengeId}/Penalties")]
	public async Task<IActionResult> ListByChallengeId(Guid challengeId)
	{
		var command = new ListPenaltiesByChallengeIdCommand(challengeId);
		var result = await listPenaltiesByChallengeIdUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpGet("Challenges/{challengeId}/Members/{userId}/Penalties")]
	[HttpGet("Challenges/{challengeId}/Users/{userId}/Penalties")]
	public async Task<IActionResult> ListByChallengeMember(Guid challengeId, Guid userId)
	{
		var command = new ListPenaltiesByChallengeMemberCommand(challengeId, userId);
		var result = await listPenaltiesByChallengeMemberUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpPost("Challenges/{challengeId}/Penalties/{penaltyId}/Pay")]
	public async Task<IActionResult> PayPenalty(Guid challengeId, Guid penaltyId, [FromBody] PayPenaltyRequest request)
	{
		var command = new PayPenaltyCommand(challengeId, penaltyId, request.ConfirmedByUserId);
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

	[HttpPost("Challenges/{challengeId}/Penalties/{penaltyId}/Pix")]
	public async Task<IActionResult> CreatePixCharge(Guid challengeId, Guid penaltyId, [FromBody] CreatePixChargeRequest request)
	{
		var command = new CreatePixChargeCommand
		{
			ChallengeId = challengeId,
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
