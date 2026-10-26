using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.Habits.ListHabitsByFilter;

public sealed class ListHabitsByFilterUseCase(
	IHabitRepository habitRepository,
	IChallengeRepository challengeRepository,
	IUserRepository userRepository
	)
	: IUseCase<ListHabitsByFilterCommand>
{
	public async Task<IResult> HandleAsync(ListHabitsByFilterCommand command)
	{
		try
		{
			command.Validate();

			var habits = await habitRepository.ListAllAsync();

			if (command.Name is not null)
			{
				habits = habits
					.Where(habit =>
						habit.Name.Contains(command.Name, StringComparison.CurrentCultureIgnoreCase))
					.OrderBy(habit => habit.Name.IndexOf(command.Name))
					.ThenBy(habit => habit.Name)
					.ToList();
			}

			var habitModels = await Task.WhenAll(
				habits.Select(async habit =>
				{
					var challenges = await challengeRepository.ListByHabitIdAsync(habit.Id);
					var ownerIds = challenges.Select(challenge => challenge.OwnerId).Distinct().ToList();
					var owners = ownerIds.Count == 0
						? []
						: await userRepository.BatchGetByIdAsync(ownerIds);

					if (ownerIds.Count > owners.Count)
						throw new Exception(Messages.UserNotFound);

					return habit.ToModel(challenges, owners);
				})
			);

			return Result.Success(habitModels);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
