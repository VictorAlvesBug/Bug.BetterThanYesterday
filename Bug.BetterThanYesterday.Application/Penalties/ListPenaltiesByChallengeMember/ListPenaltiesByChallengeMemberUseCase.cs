using Bug.BetterThanYesterday.Application.Penalties;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Penalties;
using Bug.BetterThanYesterday.Domain.ChallengeMembers;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.Entities;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Penalties.ListPenaltiesByChallengeMember;

public sealed class ListPenaltiesByChallengeMemberUseCase(
	IPenaltyRepository penaltyRepository,
	IChallengeRepository challengeRepository,
	IChallengeMemberRepository challengeMemberRepository)
	: IUseCase<ListPenaltiesByChallengeMemberCommand>
{
	public async Task<IResult> HandleAsync(ListPenaltiesByChallengeMemberCommand command)
	{
		try
		{
			command.Validate();

			var challenge = await challengeRepository.GetByIdAsync(command.ChallengeId);

			if (challenge is null)
				return Result.Rejected(Messages.ChallengeNotFound, RejectionType.NotFound);

			var challengeMemberId = ChallengeMember.BuildId(command.ChallengeId, command.UserId);
			var challengeMember = await challengeMemberRepository.GetByIdAsync(challengeMemberId);

			if (challengeMember is null)
				return Result.Rejected(Messages.ChallengeMemberNotFound, RejectionType.NotFound);

			var penalties = await penaltyRepository.ListByChallengeIdAndUserIdAsync(command.ChallengeId, command.UserId);

			return Result.Success(
				penalties.Select(PenaltyMapper.ToModel).ToList(),
				Messages.PenaltiesSuccessfullyFound);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
