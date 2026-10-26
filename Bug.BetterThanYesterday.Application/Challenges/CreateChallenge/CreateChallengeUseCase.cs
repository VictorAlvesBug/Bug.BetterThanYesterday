using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Challenges.Entities;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.Challenges.CreateChallenge;

public class CreateChallengeUseCase(
	IUserRepository userRepository,
	IChallengeRepository challengeRepository,
	IHabitRepository habitRepository)
	: IUseCase<CreateChallengeCommand>
{
	public async Task<IResult> HandleAsync(CreateChallengeCommand command)
	{
		try
		{
			command.Validate();
			
			var owner = await userRepository.GetByIdAsync(command.OwnerId);

			if (owner is null)
				return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);
			
			var habit = await habitRepository.GetByIdAsync(command.HabitId);

			if (habit is null)
				return Result.Rejected(Messages.HabitNotFound, RejectionType.NotFound);

			var challenge = Challenge.CreateNew(
				command.OwnerId,
				command.HabitId,
				command.Description,
				command.StartsAt,
				command.EndsAt,
				command.Type,
				command.DaysOffPerWeek,
				command.PenaltyValue,
				command.CoverPhotoUrl);

			await challengeRepository.AddAsync(challenge);
			return Result.Success(challenge.ToModel(habit, owner), Messages.ChallengeSuccessfullyRegistered);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
