using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Habits.Policies;

namespace Bug.BetterThanYesterday.Application.Habits.DeleteHabit;

public class DeleteHabitUseCase(
	IHabitRepository habitRepository,
	IHabitDeletionPolicy habitDeletionPolicy)
	: IUseCase<DeleteHabitCommand>
{
	public async Task<IResult> HandleAsync(DeleteHabitCommand command)
	{
		command.Validate();
		var habit = await habitRepository.GetByIdAsync(command.HabitId);

		if (habit is null)
			return Result.Rejected("Hábito não encontrado");

		if (!await habitDeletionPolicy.CanDeleteAsync(habit.Id))
			return Result.Rejected("Hábito não pode ser removido, pois possui planos vinculados");

		await habitRepository.DeleteAsync(habit);
		return Result.Success("Hábito deletado com sucesso");
	}
}
