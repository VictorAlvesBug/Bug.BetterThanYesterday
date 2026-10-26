using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.Challenges.ListChallengesByHabitId;

public class ListChallengesByHabitIdUseCase(
	IChallengeRepository challengeRepository,
	IHabitRepository habitRepository,
	IUserRepository userRepository)
	: IUseCase<ListChallengesByHabitIdCommand>
{
	public async Task<IResult> HandleAsync(ListChallengesByHabitIdCommand command)
	{
		try
		{
			command.Validate();
			
			var habit = await habitRepository.GetByIdAsync(command.HabitId);

			if (habit is null)
				return Result.Rejected(Messages.HabitNotFound, RejectionType.NotFound);

			var challenges = await challengeRepository.ListByHabitIdAsync(command.HabitId);
			var ownerIds = challenges.Select(challenge => challenge.OwnerId).Distinct().ToList();
			var owners = ownerIds.Count == 0
				? []
				: await userRepository.BatchGetByIdAsync(ownerIds);

			if (ownerIds.Count > owners.Count)
				return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

			var ownersById = owners.ToDictionary(owner => owner.Id);
			var challengeModels = challenges.Select(challenge => challenge.ToModel(habit, ownersById[challenge.OwnerId]));

			return Result.Success(
				challengeModels,
				Messages.ChallengesSuccessfullyFound
			);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
