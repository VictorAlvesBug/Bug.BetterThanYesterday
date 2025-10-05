using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
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
			return Result.Rejected("Já existe um hábito cadastrado com esse nome");

		var habit = Habit.CreateNew(command.Name);
		await habitRepository.AddAsync(habit);
		return Result.Success(habit.ToModel(), "Hábito cadastrado com sucesso");
	}
}
