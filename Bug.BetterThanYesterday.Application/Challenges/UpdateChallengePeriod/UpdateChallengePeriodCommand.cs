using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Challenges.UpdateChallengePeriod;

public class UpdateChallengePeriodCommand : ICommand
{
	public required Guid ChallengeId { get; init; }
	public required Guid OwnerId { get; init; }
	public required DateTime StartsAt { get; init; }
	public required DateTime EndsAt { get; init; }

	public void Validate()
	{
		if (ChallengeId == Guid.Empty)
			throw new ArgumentNullException(nameof(ChallengeId), Messages.EnterChallengeId);

		if (OwnerId == Guid.Empty)
			throw new ArgumentNullException(nameof(OwnerId), Messages.EnterOwnerId);

		if (StartsAt == DateTime.MinValue)
			throw new ArgumentException(Messages.EnterChallengeStartDate, nameof(StartsAt));

		if (EndsAt == DateTime.MinValue)
			throw new ArgumentException(Messages.EnterChallengeEndDate, nameof(EndsAt));
	}
}
