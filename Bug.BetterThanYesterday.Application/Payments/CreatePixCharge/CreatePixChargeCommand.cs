using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Payments.CreatePixCharge;

public sealed class CreatePixChargeCommand : ICommand
{
	public required Guid ChallengeId { get; init; }
	public required Guid PenaltyId { get; init; }
	public required Guid UserId { get; init; }

	public void Validate()
	{
		if (ChallengeId == Guid.Empty)
			throw new ArgumentNullException(nameof(ChallengeId), Messages.EnterChallengeId);

		if (PenaltyId == Guid.Empty)
			throw new ArgumentNullException(nameof(PenaltyId), Messages.EnterPenaltyId);

		if (UserId == Guid.Empty)
			throw new ArgumentNullException(nameof(UserId), Messages.EnterUserId);
	}
}
