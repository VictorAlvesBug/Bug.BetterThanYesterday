using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Habits;

namespace Bug.BetterThanYesterday.Application.Habits.UpdateHabit;

public sealed class UpdateHabitUseCase(
	IHabitRepository habitRepository)
	: IUseCase<UpdateHabitCommand>
{
	public async Task<IResult> HandleAsync(UpdateHabitCommand command)
	{
		command.Validate();
		var habit = await habitRepository.GetByIdAsync(command.HabitId);

		if (habit is null)
			return Result.Rejected(Messages.HabitNotFound);

		var existingNameHabit = await habitRepository.GetByNameAsync(command.Name);

		if (existingNameHabit is not null 
			&& existingNameHabit.Id != habit.Id)
		{
			return Result.Rejected(Messages.ThereIsAlreadyAHabitRegisteredWithThatName);
		}

		habit.UpdateName(command.Name);

		await habitRepository.UpdateAsync(habit);
		return Result.Success(habit.ToModel(), Messages.HabitSuccessfullyUpdated);
	}
}
