using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;

namespace Bug.BetterThanYesterday.Application.Habits.ListAllHabits;

public sealed class ListAllHabitsUseCase(IHabitRepository habitRepository)
	: IUseCase<ListAllHabitsCommand, IResult>
{
	public async Task<IResult> HandleAsync(ListAllHabitsCommand input)
	{
		try
		{
			var habits = (await habitRepository.ListAllAsync()).Select(h => h.ToModel());
			return Result.Success(habits);
		}
		catch (Exception ex)
		{
			return Result.Failure(ex.Message);
		}
	}
}
