using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Plans.CreatePlan;

public class CreatePlanCommand : ICommand
{
	public CreatePlanCommand(
		Guid ownerId,
		Guid habitId,
		string description,
		DateTime startsAt,
		DateTime endsAt,
		string type,
		int daysOffPerWeek,
		decimal penaltyValue)
	{
		OwnerId = ownerId;
		HabitId = habitId;
		Description = description;
		StartsAt = startsAt;
		EndsAt = endsAt;
		Type = type;
		DaysOffPerWeek = daysOffPerWeek;
		PenaltyValue = penaltyValue;
	}

	public Guid OwnerId { get; init; }
	public Guid HabitId { get; init; }
	public string? Description { get; init; }
	public DateTime StartsAt { get; init; }
	public DateTime EndsAt { get; init; }
	public string Type { get; init; }
	public int DaysOffPerWeek { get; set; }
	public decimal PenaltyValue { get; set; }

	public void Validate()
	{
		if (OwnerId == Guid.Empty)
			throw new ArgumentNullException(nameof(OwnerId), Messages.EnterOwnerId);

		if (HabitId == Guid.Empty)
			throw new ArgumentNullException(nameof(HabitId), Messages.EnterHabitId);

		if (StartsAt == DateTime.MinValue)
			throw new ArgumentException(Messages.EnterPlanStartDate, nameof(StartsAt));

		if (EndsAt == DateTime.MinValue)
			throw new ArgumentException(Messages.EnterPlanEndDate, nameof(EndsAt));

		if (string.IsNullOrWhiteSpace(Type))
			throw new ArgumentException(Messages.EnterPlanType, nameof(Type));
	}
}
