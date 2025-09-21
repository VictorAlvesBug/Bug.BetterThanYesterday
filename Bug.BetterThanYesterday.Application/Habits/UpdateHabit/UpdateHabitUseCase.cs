using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;

namespace Bug.BetterThanYesterday.Application.Habits.UpdateHabit;

public sealed class UpdateHabitUseCase(
	IHabitRepository habitRepository)
	: IUseCase<UpdateHabitCommand, IResult>
{
	public async Task<IResult> HandleAsync(UpdateHabitCommand command)
	{
		try
		{
			command.Validate();
			var habit = await habitRepository.GetByIdAsync(command.Id);

			if(habit is null)
				return Result.Rejected("Hábito não encontrado");

			habit.UpdateName(command.Name);

			await habitRepository.UpdateAsync(habit);
			return Result.Success(habit.ToModel(), "Hábito atualizado com sucesso");
		}
		catch (Exception ex)
		{
			return Result.Failure(ex.Message);
		}
	}
}
