using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Habits.DeleteHabit;

public class DeleteHabitCommand : ICommand
{
	public DeleteHabitCommand(Guid id) => Id = id;

	public Guid Id { get; init; }

	public void Validate()
	{
		if (Id == Guid.Empty)
			throw new ArgumentNullException(nameof(Id), "Informe o ID do hábito");
	}
}
