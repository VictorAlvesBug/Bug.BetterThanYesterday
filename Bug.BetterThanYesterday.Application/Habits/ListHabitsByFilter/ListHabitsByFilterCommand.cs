using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Habits.ListAllHabits;

public class ListHabitsByFilterCommand : ICommand
{
	public string? Name { get; init; }
	public void Validate()
	{
	}
}
