using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Habits.GetHabitById;

public class GetHabitByIdCommand : ICommand
{
	public GetHabitByIdCommand(Guid habitId) => HabitId = habitId;

	public Guid HabitId { get; init; }

	public void Validate()
	{
		if (HabitId == Guid.Empty)
			throw new ArgumentNullException(nameof(HabitId), "Informe o ID do hábito");
	}
}
