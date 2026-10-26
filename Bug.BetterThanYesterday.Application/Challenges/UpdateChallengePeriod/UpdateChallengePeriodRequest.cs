namespace Bug.BetterThanYesterday.Application.Challenges.UpdateChallengePeriod;

public class UpdateChallengePeriodRequest
{
	public required Guid OwnerId { get; init; }
	public required DateTime StartsAt { get; init; }
	public required DateTime EndsAt { get; init; }
}
