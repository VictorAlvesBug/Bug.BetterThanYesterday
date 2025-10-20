using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Domain.Habits.Entities;

public class Habit : Entity
{
	public string Name { get; set; }
	public DateOnly CreatedAt { get; set; }

	private Habit(
		Guid id,
		string name,
		DateTime createdAt)
	{
		Id = id;
		Name = name;
		CreatedAt = DateOnly.FromDateTime(createdAt);
	}

	private Habit(string name)
		: this(
			id: Guid.NewGuid(),
			name,
			createdAt: DateTime.Today)
	{
	}

	public static Habit CreateNew(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentNullException(nameof(name), Messages.EnterHabitName);
		
		return new Habit(name);
	}

	public static Habit Restore(
		Guid id,
		string name,
		DateTime createdAt)
	{
		if (id == Guid.Empty)
			throw new ArgumentNullException(nameof(id), Messages.EnterHabitId);

		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentNullException(nameof(name), Messages.EnterHabitName);

		if (createdAt == DateTime.MinValue)
			throw new ArgumentNullException(nameof(createdAt), Messages.EnterHabitCreationDate);

		return new Habit(
			id,
			name,
			createdAt);
	}

	public void UpdateName(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentNullException(nameof(name), Messages.EnterHabitName);
		Name = name;
	}
}
