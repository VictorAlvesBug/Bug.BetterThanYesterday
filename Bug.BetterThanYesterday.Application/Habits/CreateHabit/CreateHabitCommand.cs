using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Habits.CreateHabit;

public class CreateHabitCommand : ICommand
{
	public CreateHabitCommand(string name) => Name = name;

	public string Name { get; }

	public void Validate()
	{
		if (string.IsNullOrWhiteSpace(Name))
			throw new ArgumentNullException(nameof(Name), "Informe o nome do hábito");
	}
}
