namespace Bug.BetterThanYesterday.Domain.Notifications.ValueObjects;

public sealed class CheckInReportedPayload : NotificationPayload
{
	public Guid CheckInId { get; init; }
	public Guid ReporterId { get; init; }
	public Guid TargetUserId { get; init; }
}
