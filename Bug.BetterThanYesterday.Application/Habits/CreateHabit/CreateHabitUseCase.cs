using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Habits.Entities;

namespace Bug.BetterThanYesterday.Application.Habits.CreateHabit;

public sealed class CreateHabitUseCase(IHabitRepository habitRepository)
	: IUseCase<CreateHabitCommand>
{
	public async Task<IResult> HandleAsync(CreateHabitCommand command)
	{
		command.Validate();
		var alreadyExists = (await habitRepository.GetByNameAsync(command.Name)) is not null;

		if (alreadyExists)
			return Result.Rejected(Messages.ThereIsAlreadyAHabitRegisteredWithThatName);
	
		try
		{
			var habit = Habit.CreateNew(command.Name);
			await habitRepository.AddAsync(habit);
			return Result.Success(habit.ToModel(), Messages.HabitSuccessfullyRegistered);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
