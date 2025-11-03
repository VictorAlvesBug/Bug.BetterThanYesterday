using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Strings;
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
		try
		{
			command.Validate();

			var habit = await habitRepository.GetByIdAsync(command.HabitId);

			if (habit is null)
				return Result.Rejected(Messages.HabitNotFound);

			if (!await habitDeletionPolicy.CanDeleteAsync(habit.Id))
				return Result.Rejected(Messages.HabitCannotBeRemovedAsItHasLinkedPlans);

			await habitRepository.DeleteAsync(habit);
			return Result.Success(Messages.HabitSuccessfullyDeleted);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
