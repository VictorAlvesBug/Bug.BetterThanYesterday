using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Plans.UpdatePlanStatus;

public class UpdatePlanStatusCommand : ICommand
{
	public UpdatePlanStatusCommand(Guid planId, int statusId)
	{
		PlanId = planId;
		StatusId = statusId;
	}

	public Guid PlanId { get; init; }
	public int StatusId { get; init; }

	public void Validate()
	{
		if (PlanId == Guid.Empty)
			throw new ArgumentNullException(nameof(PlanId), Messages.EnterPlanId);

		if (StatusId <= 0)
			throw new ArgumentException(Messages.EnterPlanStatus, nameof(StatusId));
	}
}
