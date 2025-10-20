using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Plans.CancelPlan;

public class CancelPlanCommand : ICommand
{
	public CancelPlanCommand(Guid planId) => PlanId = planId;

	public Guid PlanId { get; init; }

	public void Validate()
	{
		if (PlanId == Guid.Empty)
			throw new ArgumentNullException(nameof(PlanId), Messages.EnterPlanId);
	}
}
