using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.ChallengeMembers;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.Challenges.UpdateChallengePeriod;

public sealed class UpdateChallengePeriodUseCase(
	IChallengeRepository challengeRepository,
	IChallengeMemberRepository challengeMemberRepository,
	IHabitRepository habitRepository,
	IUserRepository userRepository)
	: IUseCase<UpdateChallengePeriodCommand>
{
	public async Task<IResult> HandleAsync(UpdateChallengePeriodCommand command)
	{
		try
		{
			command.Validate();

			var challenge = await challengeRepository.GetByIdAsync(command.ChallengeId);

			if (challenge is null)
				return Result.Rejected(Messages.ChallengeNotFound, RejectionType.NotFound);

			if (challenge.OwnerId != command.OwnerId)
				return Result.Rejected(Messages.EnterOwnerId);

			var habit = await habitRepository.GetByIdAsync(challenge.HabitId);

			if (habit is null)
				return Result.Rejected(Messages.HabitNotFound, RejectionType.NotFound);

			var owner = await userRepository.GetByIdAsync(challenge.OwnerId);

			if (owner is null)
				return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

			var members = await challengeMemberRepository.ListByChallengeIdAsync(challenge.Id);
			challenge.UpdatePeriod(
				DateOnly.FromDateTime(command.StartsAt),
				DateOnly.FromDateTime(command.EndsAt),
				members.Count);
			await challengeRepository.UpdateAsync(challenge);

			return Result.Success(
				challenge.ToModel(habit, owner, members.Count),
				Messages.ChallengeSuccessfullyRegistered);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
