using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Habits.Entities;

namespace Bug.BetterThanYesterday.Application.Habits.CreateHabit;

public sealed class CreateHabitUseCase(IHabitRepository habitRepository)
	: IUseCase<CreateHabitCommand, IResult>
{
	public async Task<IResult> HandleAsync(CreateHabitCommand command)
	{
		try
		{
			var habit = Habit.CreateNew(command.Name);
			await habitRepository.AddAsync(habit);
			return Result.Success(habit.ToModel(), "Hábito cadastrado com sucesso.");
		}
		catch (Exception ex)
		{
			return Result.Failure(ex.Message);
		}
	}
}
