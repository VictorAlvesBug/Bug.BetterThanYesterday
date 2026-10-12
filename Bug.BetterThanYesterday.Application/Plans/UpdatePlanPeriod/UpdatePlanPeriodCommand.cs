using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Plans.UpdatePlanPeriod;

public class UpdatePlanPeriodCommand : ICommand
{
	public required Guid PlanId { get; init; }
	public required Guid OwnerId { get; init; }
	public required DateTime StartsAt { get; init; }
	public required DateTime EndsAt { get; init; }

	public void Validate()
	{
		if (PlanId == Guid.Empty)
			throw new ArgumentNullException(nameof(PlanId), Messages.EnterPlanId);

		if (OwnerId == Guid.Empty)
			throw new ArgumentNullException(nameof(OwnerId), Messages.EnterOwnerId);

		if (StartsAt == DateTime.MinValue)
			throw new ArgumentException(Messages.EnterPlanStartDate, nameof(StartsAt));

		if (EndsAt == DateTime.MinValue)
			throw new ArgumentException(Messages.EnterPlanEndDate, nameof(EndsAt));
	}
}
