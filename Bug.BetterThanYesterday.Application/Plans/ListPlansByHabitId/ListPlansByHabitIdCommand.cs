using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Plans.ListPlansByHabitId;

public class ListPlansByHabitIdCommand : ICommand
{
	public ListPlansByHabitIdCommand(string habitId) => HabitId = habitId;

	public string HabitId { get; }

	public void Validate()
	{
		if (string.IsNullOrWhiteSpace(HabitId))
			throw new ArgumentNullException(nameof(HabitId), "Informe o ID do hábito");
	}
}
