namespace Bug.BetterThanYesterday.Application.Plans.UpdatePlanPeriod;

public class UpdatePlanPeriodRequest
{
	public required Guid OwnerId { get; init; }
	public required DateTime StartsAt { get; init; }
	public required DateTime EndsAt { get; init; }
}
