using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.Challenges;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

using Bug.BetterThanYesterday.Domain.ChallengeMembers;

namespace Bug.BetterThanYesterday.Application.Challenges.GetChallengeById;

public class GetChallengeByIdUseCase(
	IChallengeRepository challengeRepository,
	IHabitRepository habitRepository,
	IUserRepository userRepository,
	IChallengeMemberRepository challengeMemberRepository)
	: IUseCase<GetChallengeByIdCommand>
{
	public async Task<IResult> HandleAsync(GetChallengeByIdCommand command)
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

			var members = await challengeMemberRepository.ListByChallengeIdAsync(challenge.Id);
			await ChallengeQuorumPostponer.PostponeWhileUnderQuorumAsync(challenge, members.Count, challengeRepository);

			return Result.Success(
				challenge.ToModel(habit, owner, members.Count),
				Messages.ChallengeSuccessfullyFound
			);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
