namespace Bug.BetterThanYesterday.Domain.Notifications.ValueObjects;

public sealed class CheckInMadePayload : NotificationPayload
{
	public Guid CheckInId { get; init; }
	public Guid AuthorId { get; init; }
}
