using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Habits.DeleteHabit;

public class DeleteHabitCommand : ICommand
{
	public DeleteHabitCommand(string id) => Id = id;

	public string Id { get; }

	public void Validate()
	{
		if (string.IsNullOrWhiteSpace(Id))
			throw new ArgumentNullException(nameof(Id), "Informe o ID do hábito");
	}
}
