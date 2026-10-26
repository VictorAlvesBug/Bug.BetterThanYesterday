using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Microsoft.AspNetCore.Mvc;
using Bug.BetterThanYesterday.Application.ChallengeMembers;
using Bug.BetterThanYesterday.Application.ChallengeMembers.AddUserToChallenge;
using Bug.BetterThanYesterday.Application.ChallengeMembers.GetChallengeMemberDetails;
using Bug.BetterThanYesterday.Application.ChallengeMembers.GetChallengeWithMembersByChallengeId;
using Bug.BetterThanYesterday.Application.ChallengeMembers.GetUserWithChallengesByUserId;
using Bug.BetterThanYesterday.Application.ChallengeMembers.BlockUserInTheChallenge;
using Bug.BetterThanYesterday.Application.ChallengeMembers.UnblockUserInTheChallenge;
using Bug.BetterThanYesterday.Application.ChallengeMembers.RemoveUserFromChallenge;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bug.BetterThanYesterday.API.Controllers;

[Route("api")]
[Route("testapi")]
[ApiController]
public class ChallengeMembersController(
	IUseCase<GetChallengeMemberDetailsCommand> getChallengeMemberDetailsUseCase,
	IUseCase<GetChallengeWithMembersByChallengeIdCommand> getChallengeWithMembersByChallengeIdUseCase,
	IUseCase<GetUserWithChallengesByUserIdCommand> getUserWithChallengesByUserIdUseCase,
	IUseCase<BlockUserInTheChallengeCommand> blockUserInTheChallengeUseCase,
	IUseCase<UnblockUserInTheChallengeCommand> unblockUserInTheChallengeUseCase,
	IUseCase<AddUserToChallengeCommand> addUserToChallengeUseCase,
	IUseCase<RemoveUserFromChallengeCommand> removeUserFromChallengeUseCase)
	: ControllerBase
{
	[HttpGet("Challenges/{challengeId}/Members")]
	[HttpGet("Challenges/{challengeId}/Users")]
	public async Task<IActionResult> GetByChallengeId(Guid challengeId)
	{
		var command = new GetChallengeWithMembersByChallengeIdCommand(challengeId);
		var result = await getChallengeWithMembersByChallengeIdUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}
	
	[HttpGet("Users/{userId}/Challenges")]
	public async Task<IActionResult> GetByUserId(Guid userId)
	{
		var command = new GetUserWithChallengesByUserIdCommand(userId);
		var result = await getUserWithChallengesByUserIdUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpGet("Challenges/{challengeId}/Members/{userId}")]
	[HttpGet("Challenges/{challengeId}/Users/{userId}")]
	//[HttpGet("Users/{userId}/Challenges/{challengeId}")]
	public async Task<IActionResult> GetByChallengeIdAndUserId(Guid challengeId, Guid userId)
	{
		var command = new GetChallengeMemberDetailsCommand(challengeId, userId);
		var result = await getChallengeMemberDetailsUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpPost("Challenges/{challengeId}/Members/{userId}")]
	[HttpPost("Challenges/{challengeId}/Users/{userId}")]
	//[HttpPost("Users/{userId}/Challenges/{challengeId}")]
	public async Task<IActionResult> AddUserToChallenge(Guid challengeId, Guid userId)
	{
		var command = new AddUserToChallengeCommand(challengeId, userId);
		var result = await addUserToChallengeUseCase.HandleAsync(command);

		if (result.IsSuccess())
		{
			var data = ((Result<ChallengeMemberDetailsModel>)result).Data;
			return Created($"Challenges/{data.Challenge.Id}/Members/{data.User.Id}", result);
		}

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpDelete("Challenges/{challengeId}/Members/{userId}")]
	[HttpDelete("Challenges/{challengeId}/Users/{userId}")]
	//[HttpDelete("Users/{userId}/Challenges/{challengeId}")]
	public async Task<IActionResult> RemoveUserFromChallenge(Guid challengeId, Guid userId)
	{
		var command = new RemoveUserFromChallengeCommand(challengeId, userId);
		var result = await removeUserFromChallengeUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return NoContent();

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpPost("Challenges/{challengeId}/Members/{userId}/Block")]
	[HttpPost("Challenges/{challengeId}/Users/{userId}/Block")]
	//[HttpPost("Users/{userId}/Challenges/{challengeId}/Block")]
	public async Task<IActionResult> BlockUserInTheChallenge(Guid challengeId, Guid userId)
	{
		var command = new BlockUserInTheChallengeCommand(challengeId, userId);
		var result = await blockUserInTheChallengeUseCase.HandleAsync(command);

		if (result.IsSuccess())
		{
			var data = ((Result<ChallengeMemberDetailsModel>)result).Data;
			return Created($"Challenges/{data.Challenge.Id}/Members/{data.User.Id}/Block", result);
		}

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpDelete("Challenges/{challengeId}/Members/{userId}/Block")]
	[HttpDelete("Challenges/{challengeId}/Users/{userId}/Block")]
	//[HttpDelete("Users/{userId}/Challenges/{challengeId}/Block")]
	public async Task<IActionResult> UnblockUserInTheChallenge(Guid challengeId, Guid userId)
	{
		var command = new UnblockUserInTheChallengeCommand(challengeId, userId);
		var result = await unblockUserInTheChallengeUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}
}
