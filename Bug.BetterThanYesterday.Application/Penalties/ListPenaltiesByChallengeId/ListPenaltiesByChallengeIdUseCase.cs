using Bug.BetterThanYesterday.Application.Penalties;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Penalties;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Penalties.ListPenaltiesByChallengeId;

public sealed class ListPenaltiesByChallengeIdUseCase(
	IPenaltyRepository penaltyRepository,
	IChallengeRepository challengeRepository)
	: IUseCase<ListPenaltiesByChallengeIdCommand>
{
	public async Task<IResult> HandleAsync(ListPenaltiesByChallengeIdCommand command)
	{
		try
		{
			command.Validate();

			var challenge = await challengeRepository.GetByIdAsync(command.ChallengeId);

			if (challenge is null)
				return Result.Rejected(Messages.ChallengeNotFound, RejectionType.NotFound);

			var penalties = await penaltyRepository.ListByChallengeIdAsync(command.ChallengeId);

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
