using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Habits;

namespace Bug.BetterThanYesterday.Application.Habits.GetHabitById;

public sealed class GetHabitByIdUseCase(IHabitRepository habitRepository)
	: IUseCase<GetHabitByIdCommand>
{
	public async Task<IResult> HandleAsync(GetHabitByIdCommand command)
	{
		command.Validate();
		var habit = await habitRepository.GetByIdAsync(command.HabitId);

		if (habit is null)
			return Result.Rejected(Messages.HabitNotFound);

		return Result.Success(habit.ToModel(), Messages.HabitSuccessfullyFound);
	}
}
