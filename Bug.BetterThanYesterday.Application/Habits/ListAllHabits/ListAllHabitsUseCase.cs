using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Habits.ListAllHabits;

public sealed class ListAllHabitsUseCase(
	IHabitRepository habitRepository,
	IPlanRepository planRepository
	)
	: IUseCase<ListAllHabitsCommand>
{
	public async Task<IResult> HandleAsync(ListAllHabitsCommand command)
	{
		try
		{
			command.Validate();

			var habitsList = await habitRepository.ListAllAsync();

			var habitTasks = habitsList.Select(async habit =>
			{
				var plans = await planRepository.ListByHabitIdAsync(habit.Id);
				return habit.ToModel(plans.Count());
			});

			var habits = await Task.WhenAll(habitTasks);

			return Result.Success(habits, Messages.HabitsSuccessfullyFound);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
