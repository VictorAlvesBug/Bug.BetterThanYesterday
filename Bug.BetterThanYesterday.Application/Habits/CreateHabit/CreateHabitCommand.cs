namespace Bug.BetterThanYesterday.Application.Habits.CreateHabit;

public class CreateHabitCommand
{
	public CreateHabitCommand(string name)
	{
		Name = name;
	}

	public string Name { get; }
}
