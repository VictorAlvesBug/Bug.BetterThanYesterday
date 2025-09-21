using Bug.BetterThanYesterday.Domain.Commons;

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
			throw new ArgumentNullException(nameof(name), "Informe o nome do hábito");
		
		return new Habit(name);
	}

	public static Habit Restore(
		Guid id,
		string name,
		DateTime createdAt)
	{
		if (id == Guid.Empty)
			throw new ArgumentNullException(nameof(id), "Informe o ID do hábito");

		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentNullException(nameof(name), "Informe o nome do hábito");

		if (createdAt == DateTime.MinValue)
			throw new ArgumentNullException(nameof(createdAt), "Informe a data de criação do hábito");

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
