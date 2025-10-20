using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Habits.CreateHabit;

public class CreateHabitCommand : ICommand
{
	public CreateHabitCommand(string name) => Name = name;

	public string Name { get; init; }

	public void Validate()
	{
		if (string.IsNullOrWhiteSpace(Name))
			throw new ArgumentNullException(nameof(Name), Messages.EnterHabitName);
	}
}
