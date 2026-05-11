using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Habits.ListHabitsByFilter;

public sealed class ListHabitsByFilterUseCase(
	IHabitRepository habitRepository,
	IPlanRepository planRepository
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
					var plans = await planRepository.ListByHabitIdAsync(habit.Id);
					return habit.ToModel(plans);
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
