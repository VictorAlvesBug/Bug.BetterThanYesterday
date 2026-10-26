using Bug.BetterThanYesterday.Application.Challenges.CreateChallenge;
using Bug.BetterThanYesterday.Application.Challenges.CancelChallenge;
using Bug.BetterThanYesterday.Application.Challenges.GetChallengeById;
using Bug.BetterThanYesterday.Application.Challenges.ListChallengesByFilter;
using Bug.BetterThanYesterday.Application.Challenges;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Microsoft.AspNetCore.Mvc;
using Bug.BetterThanYesterday.Application.Challenges.GetChallengeRanking;
using Bug.BetterThanYesterday.Application.Challenges.UpdateChallengePeriod;
using Bug.BetterThanYesterday.Application.Challenges.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bug.BetterThanYesterday.API.Controllers;

[Route("api/[controller]")]
[Route("testapi/[controller]")]
[ApiController]
public class ChallengesController(
	IUseCase<CreateChallengeCommand> createChallengeUseCase,
	IUseCase<ListChallengesByFilterCommand> listChallengesByFilterUseCase,
	IUseCase<GetChallengeByIdCommand> getChallengeByIdUseCase,
	IUseCase<GetChallengeRankingCommand> getChallengeRankingUseCase,
	IUseCase<UpdateChallengePeriodCommand> updateChallengePeriodUseCase,
	IUseCase<CancelChallengeCommand> cancelChallengeUseCase)
	: ControllerBase
{
	[HttpGet("{challengeId}")]
	public async Task<IActionResult> GetById(Guid challengeId)
	{
		var command = new GetChallengeByIdCommand(challengeId);
		var result = await getChallengeByIdUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpGet]
	public async Task<IActionResult> ListByFilter([FromQuery] ListChallengesByFilterCommand command)
	{
		var result = await listChallengesByFilterUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpGet("{challengeId}/Ranking")]
	public async Task<IActionResult> GetRanking(Guid challengeId, [FromQuery] Guid? userId)
	{
		var command = new GetChallengeRankingCommand(challengeId, userId);
		var result = await getChallengeRankingUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateChallengeCommand command)
	{
		var result = await createChallengeUseCase.HandleAsync(command);

		if (result.IsSuccess())
		{
			var data = ((Result<ChallengeModel>)result).Data;
			return Created($"Challenges/{data.Id}", result);
		}

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpPut("{challengeId}/Period")]
	public async Task<IActionResult> UpdatePeriod(Guid challengeId, [FromBody] UpdateChallengePeriodRequest request)
	{
		var command = new UpdateChallengePeriodCommand
		{
			ChallengeId = challengeId,
			OwnerId = request.OwnerId,
			StartsAt = request.StartsAt,
			EndsAt = request.EndsAt
		};
		var result = await updateChallengePeriodUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpDelete("{challengeId}")]
	public async Task<IActionResult> Cancel(Guid challengeId)
	{
		var command = new CancelChallengeCommand(challengeId);
		var result = await cancelChallengeUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	/*[HttpGet]
	private async Task<IActionResult> ListByHabitId(Guid habitId)
	{
		var command = new ListChallengesByHabitIdCommand(habitId);
		var result = await listChallengesByHabitIdUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}*/
}
