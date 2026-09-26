using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Penalties.PayPenalty;

public sealed class PayPenaltyCommand : ICommand
{
	public PayPenaltyCommand(Guid planId, Guid penaltyId, Guid confirmedByUserId)
	{
		PlanId = planId;
		PenaltyId = penaltyId;
		ConfirmedByUserId = confirmedByUserId;
	}

	public Guid PlanId { get; }
	public Guid PenaltyId { get; }
	public Guid ConfirmedByUserId { get; }

	public void Validate()
	{
		if (PlanId == Guid.Empty)
			throw new ArgumentException("PlanId is required", nameof(PlanId));

		if (PenaltyId == Guid.Empty)
			throw new ArgumentException("PenaltyId is required", nameof(PenaltyId));

		if (ConfirmedByUserId == Guid.Empty)
			throw new ArgumentException("ConfirmedByUserId is required", nameof(ConfirmedByUserId));
	}
}
