using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;

namespace Bug.BetterThanYesterday.Application.Habits.ListAllHabits;

public sealed class ListAllHabitsUseCase(IHabitRepository habitRepository)
	: IUseCase<ListAllHabitsCommand, IResult>
{
	public async Task<IResult> HandleAsync(ListAllHabitsCommand command)
	{
		try
		{
			command.Validate();
			var habits = (await habitRepository.ListAllAsync()).Select(habit => habit.ToModel());
			return Result.Success(habits);
		}
		catch (Exception ex)
		{
			return Result.Failure(ex.Message);
		}
	}
}
