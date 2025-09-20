using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Habits.UpdateHabit;

public class UpdateHabitCommand : ICommand
{
	public UpdateHabitCommand(string id, string name)
	{
		Id = id;
		Name = name;
	}

	public string Id { get; }
	public string Name { get; }

	public void Validate()
	{
		if (string.IsNullOrWhiteSpace(Id))
			throw new ArgumentNullException(nameof(Id), "Informe o ID do hábito");

		if (string.IsNullOrWhiteSpace(Name))
			throw new ArgumentNullException(nameof(Name), "Informe o nome do hábito");
	}
}
