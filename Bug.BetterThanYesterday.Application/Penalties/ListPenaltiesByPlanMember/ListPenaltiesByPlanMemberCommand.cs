using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Penalties.ListPenaltiesByPlanMember;

public sealed class ListPenaltiesByPlanMemberCommand : ICommand
{
	public ListPenaltiesByPlanMemberCommand(Guid planId, Guid userId)
	{
		PlanId = planId;
		UserId = userId;
	}

	public Guid PlanId { get; }
	public Guid UserId { get; }

	public void Validate()
	{
		if (PlanId == Guid.Empty)
			throw new ArgumentException("PlanId is required", nameof(PlanId));

		if (UserId == Guid.Empty)
			throw new ArgumentException("UserId is required", nameof(UserId));
	}
}
