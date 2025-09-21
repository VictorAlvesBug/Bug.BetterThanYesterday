using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Habits.UpdateHabit;

public class UpdateHabitCommand : ICommand
{
	public UpdateHabitCommand(Guid id, string name)
	{
		Id = id;
		Name = name;
	}

	public Guid Id { get; init; }
	public string Name { get; init; }

	public void Validate()
	{
		if (Id == Guid.Empty)
			throw new ArgumentNullException(nameof(Id), "Informe o ID do hábito");

		if (string.IsNullOrWhiteSpace(Name))
			throw new ArgumentNullException(nameof(Name), "Informe o nome do hábito");
	}
}
