using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Habits.GetHabitById;

public class GetHabitByIdCommand : ICommand
{
	public GetHabitByIdCommand(string id) => Id = id;

	public string Id { get; }

	public void Validate()
	{
		if (string.IsNullOrWhiteSpace(Id))
			throw new ArgumentNullException(nameof(Id), "Informe o ID do hábito");
	}
}
