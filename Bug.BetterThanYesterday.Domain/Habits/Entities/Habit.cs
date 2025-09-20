using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Domain.Habits.Entities;

public class Habit : Entity
{
	public string Name { get; set; }
	public DateOnly CreatedAt { get; set; }

	private Habit(
		string id,
		string name,
		DateOnly createdAt)
	{
		Id = id;
		Name = name;
		CreatedAt = createdAt;
	}

	private Habit(string name)
		: this(
			id: Guid.NewGuid().ToString(),
			name,
			createdAt: DateOnly.FromDateTime(DateTime.Today))
	{
	}

	public static Habit CreateNew(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentNullException(nameof(name), "Informe o nome do hábito");
		
		return new Habit(name);
	}

	public static Habit Restore(
		string id,
		string name,
		DateOnly createdAt)
	{
		if (string.IsNullOrWhiteSpace(id))
			throw new ArgumentNullException(nameof(id), "Informe o ID do hábito");

		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentNullException(nameof(name), "Informe o nome do hábito");

		return new Habit(
			id,
			name,
			createdAt);
	}

	public void UpdateName(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentNullException(nameof(name), "Informe o nome do hábito");
		Name = name;
	}
}
