using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Habits.ListHabitsByFilter;

public class ListHabitsByFilterCommand : ICommand
{
	public string? Name { get; init; }
	public void Validate()
	{
	}
}
