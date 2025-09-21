using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;

namespace Bug.BetterThanYesterday.Application.Habits.ListAllHabits;

public sealed class ListAllHabitsUseCase(IHabitRepository habitRepository)
	: IUseCase<ListAllHabitsCommand>
{
	public async Task<IResult> HandleAsync(ListAllHabitsCommand command)
	{
		command.Validate();
		var habits = (await habitRepository.ListAllAsync()).Select(habit => habit.ToModel());
		return Result.Success(habits);
	}
}
