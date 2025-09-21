using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Habits.GetHabitById;

public class GetHabitByIdCommand : ICommand
{
	public GetHabitByIdCommand(Guid id) => Id = id;

	public Guid Id { get; init; }

	public void Validate()
	{
		if (Id == Guid.Empty)
			throw new ArgumentNullException(nameof(Id), "Informe o ID do hábito");
	}
}
