using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;

namespace Bug.BetterThanYesterday.Application.Habits.DeleteHabit;

public class DeleteHabitUseCase(IHabitRepository habitRepository)
	: IUseCase<DeleteHabitCommand, IResult>
{
	public async Task<IResult> HandleAsync(DeleteHabitCommand input)
	{
		try
		{
			var habit = await habitRepository.GetByIdAsync(input.Id);

			if (habit is null)
				return Result.Rejected("Hábito não encontrado");

			await habitRepository.DeleteAsync(habit);
			return Result.Success("Hábito deletado com sucesso");
		}
		catch (Exception ex)
		{
			return Result.Failure(ex.Message);
		}
	}
}
