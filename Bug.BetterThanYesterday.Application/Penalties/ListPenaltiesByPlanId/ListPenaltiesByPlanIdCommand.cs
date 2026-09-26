using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Penalties.ListPenaltiesByPlanId;

public sealed class ListPenaltiesByPlanIdCommand : ICommand
{
	public ListPenaltiesByPlanIdCommand(Guid planId)
	{
		PlanId = planId;
	}

	public Guid PlanId { get; }

	public void Validate()
	{
		if (PlanId == Guid.Empty)
			throw new ArgumentException("PlanId is required", nameof(PlanId));
	}
}
