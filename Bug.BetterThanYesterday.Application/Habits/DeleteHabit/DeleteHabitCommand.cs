using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Habits.DeleteHabit;

public class DeleteHabitCommand : ICommand
{
	public DeleteHabitCommand(Guid habitId) => HabitId = habitId;

	public Guid HabitId { get; init; }

	public void Validate()
	{
		if (HabitId == Guid.Empty)
			throw new ArgumentNullException(nameof(HabitId), Messages.HabitNotFound);
	}
}
