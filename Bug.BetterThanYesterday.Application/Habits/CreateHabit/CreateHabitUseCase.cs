using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Habits.Entities;

namespace Bug.BetterThanYesterday.Application.Habits.CreateHabit;

public sealed class CreateHabitUseCase(IHabitRepository habits)
	: IUseCase<CreateHabitCommand, Result<HabitModel>>
{
	public async Task<Result<HabitModel>> HandleAsync(CreateHabitCommand command)
	{
		try
		{
			await habits.AddAsync(Habit.CreateNew(command.Name));
			return Result<HabitModel>.Success("Hábito cadastrado com sucesso.");
		}
		catch (Exception ex)
		{
			return Result<HabitModel>.Failure(ex.Message);
		}
	}
}
