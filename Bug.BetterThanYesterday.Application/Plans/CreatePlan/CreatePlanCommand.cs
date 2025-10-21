using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Plans.CreatePlan;

public class CreatePlanCommand : ICommand
{
	public CreatePlanCommand(
		Guid habitId,
		string description,
		DateTime startsAt,
		DateTime endsAt,
		int typeId)
	{
		HabitId = habitId;
		Description = description;
		StartsAt = startsAt;
		EndsAt = endsAt;
		TypeId = typeId;
	}

	public Guid HabitId { get; init; }
	public string Description { get; init; }
	public DateTime StartsAt { get; init; }
	public DateTime EndsAt { get; init; }
	public int TypeId { get; init; }

	public void Validate()
	{
		if (HabitId == Guid.Empty)
			throw new ArgumentNullException(nameof(HabitId), Messages.EnterHabitId);

		if (string.IsNullOrWhiteSpace(Description))
			throw new ArgumentNullException(nameof(Description), Messages.EnterPlanDescription);

		if (StartsAt == DateTime.MinValue)
			throw new ArgumentException(Messages.EnterPlanStartDate, nameof(StartsAt));

		if (EndsAt == DateTime.MinValue)
			throw new ArgumentException(Messages.EnterPlanEndDate, nameof(EndsAt));

		if (TypeId <= 0)
			throw new ArgumentException(Messages.EnterPlanType, nameof(TypeId));
	}
}
