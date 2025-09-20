using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;

namespace Bug.BetterThanYesterday.Application.Habits.UpdateHabit;

public sealed class UpdateHabitUseCase(
	IHabitRepository habitRepository)
	: IUseCase<UpdateHabitCommand, IResult>
{
	public async Task<IResult> HandleAsync(UpdateHabitCommand input)
	{
		try
		{
			var habit = await habitRepository.GetByIdAsync(input.Id);

			if(habit is null)
				return Result.Rejected("Hábito não encontrado");

			habit.UpdateName(input.Name);

			await habitRepository.UpdateAsync(habit);
			return Result.Success(habit.ToModel(), "Hábito atualizado com sucesso");
		}
		catch (Exception ex)
		{
			return Result.Failure(ex.Message);
		}
	}
}
