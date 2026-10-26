using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Penalties.PayPenalty;

public sealed class PayPenaltyCommand : ICommand
{
	public PayPenaltyCommand(Guid challengeId, Guid penaltyId, Guid confirmedByUserId)
	{
		ChallengeId = challengeId;
		PenaltyId = penaltyId;
		ConfirmedByUserId = confirmedByUserId;
	}

	public Guid ChallengeId { get; }
	public Guid PenaltyId { get; }
	public Guid ConfirmedByUserId { get; }

	public void Validate()
	{
		if (ChallengeId == Guid.Empty)
			throw new ArgumentException("ChallengeId is required", nameof(ChallengeId));

		if (PenaltyId == Guid.Empty)
			throw new ArgumentException("PenaltyId is required", nameof(PenaltyId));

		if (ConfirmedByUserId == Guid.Empty)
			throw new ArgumentException("ConfirmedByUserId is required", nameof(ConfirmedByUserId));
	}
}
