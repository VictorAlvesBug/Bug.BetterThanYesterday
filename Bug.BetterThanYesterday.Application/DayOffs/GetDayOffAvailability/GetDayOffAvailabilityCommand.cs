using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.DayOffs.GetDayOffAvailability;

public class GetDayOffAvailabilityCommand : ICommand
{
	public GetDayOffAvailabilityCommand(Guid planId, Guid userId)
	{
		PlanId = planId;
		UserId = userId;
	}

	public Guid PlanId { get; }
	public Guid UserId { get; }

	public void Validate()
	{
		if (PlanId == Guid.Empty)
			throw new ArgumentNullException(nameof(PlanId), Messages.EnterPlanId);

		if (UserId == Guid.Empty)
			throw new ArgumentNullException(nameof(UserId), Messages.EnterUserId);
	}
}
