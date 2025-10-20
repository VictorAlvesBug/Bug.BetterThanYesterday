using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Habits.UpdateHabit;

public class UpdateHabitCommand : ICommand
{
	public UpdateHabitCommand(Guid habitId, string name)
	{
		HabitId = habitId;
		Name = name;
	}

	public Guid HabitId { get; init; }
	public string Name { get; init; }

	public void Validate()
	{
		if (HabitId == Guid.Empty)
			throw new ArgumentNullException(nameof(HabitId), Messages.HabitNotFound);

		if (string.IsNullOrWhiteSpace(Name))
			throw new ArgumentNullException(nameof(Name), Messages.EnterHabitName);
	}
}
