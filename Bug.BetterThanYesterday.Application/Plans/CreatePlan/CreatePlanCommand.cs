using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Plans.CreatePlan;

public class CreatePlanCommand : ICommand
{
	public CreatePlanCommand(
		string habitId,
		string description,
		DateTime startsAt,
		DateTime endsAt,
		int type)
	{
		HabitId = habitId;
		Description = description;
		StartsAt = startsAt;
		EndsAt = endsAt;
		Type = type;
	}

	public string HabitId { get; init; }
	public string Description { get; init; }
	public DateTime StartsAt { get; init; }
	public DateTime EndsAt { get; init; }
	public int Type { get; init; }

	public void Validate()
	{
		if (string.IsNullOrWhiteSpace(HabitId))
			throw new ArgumentNullException(nameof(HabitId), "Informe o ID do hábito");

		if (string.IsNullOrWhiteSpace(Description))
			throw new ArgumentNullException(nameof(Description), "Informe a descrição do plano");

		if (StartsAt == DateTime.MinValue)
			throw new ArgumentException("Informe a data inicial do plano", nameof(StartsAt));

		if (EndsAt == DateTime.MinValue)
			throw new ArgumentException("Informe a data final do plano", nameof(EndsAt));

		if (Type <= 0)
			throw new ArgumentException("Informe o tipo do plano", nameof(Type));
	}
}
