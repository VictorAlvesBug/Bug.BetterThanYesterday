using Bug.BetterThanYesterday.Application.Ledger;
using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.ChallengeMembers;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.Challenges.CancelChallenge;

public class CancelChallengeUseCase(
	IChallengeRepository challengeRepository,
	IChallengeMemberRepository challengeMemberRepository,
	IHabitRepository habitRepository,
	IUserRepository userRepository,
	IChallengeFinancesProcessor challengeFinancesProcessor)
	: IUseCase<CancelChallengeCommand>
{
	public async Task<IResult> HandleAsync(CancelChallengeCommand command)
	{
		try
		{
			command.Validate();
		
			var challenge = await challengeRepository.GetByIdAsync(command.ChallengeId);

			if (challenge is null)
				return Result.Rejected(Messages.ChallengeNotFound, RejectionType.NotFound);

			var habit = await habitRepository.GetByIdAsync(challenge.HabitId);

			if (habit is null)
				return Result.Rejected(Messages.HabitNotFound, RejectionType.NotFound);

			var owner = await userRepository.GetByIdAsync(challenge.OwnerId);

			if (owner is null)
				return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

			challenge.Cancel();

			await challengeRepository.UpdateAsync(challenge);
			await challengeFinancesProcessor.ProcessAsync(challenge.Id);

			var members = await challengeMemberRepository.ListByChallengeIdAsync(challenge.Id);
			foreach (var member in members)
				await challengeMemberRepository.DeleteAsync(member);

			return Result.Success(challenge.ToModel(habit, owner), Messages.ChallengeSuccessfullyCancelled);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
