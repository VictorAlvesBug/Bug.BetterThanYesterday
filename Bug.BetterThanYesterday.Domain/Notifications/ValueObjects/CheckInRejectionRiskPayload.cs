namespace Bug.BetterThanYesterday.Domain.Notifications.ValueObjects;

public sealed class CheckInRejectionRiskPayload : NotificationPayload
{
	public Guid CheckInId { get; init; }
}
