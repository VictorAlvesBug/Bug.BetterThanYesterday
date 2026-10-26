using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Domain.Notifications.ValueObjects;

public sealed class NotificationKind : Enumeration<NotificationKind>
{
	public static readonly NotificationKind CheckInMade = new(1, nameof(CheckInMade));
	public static readonly NotificationKind RestAtRisk = new(2, nameof(RestAtRisk));
	public static readonly NotificationKind FineAtRisk = new(3, nameof(FineAtRisk));
	public static readonly NotificationKind ChallengeStartsToday = new(4, nameof(ChallengeStartsToday));
	public static readonly NotificationKind ChallengeCancelledForQuorum = new(5, nameof(ChallengeCancelledForQuorum));
	public static readonly NotificationKind CheckInReviewed = new(6, nameof(CheckInReviewed));
	public static readonly NotificationKind CheckInRejectionRisk = new(7, nameof(CheckInRejectionRisk));
	public static readonly NotificationKind CheckInReported = new(8, nameof(CheckInReported));

	private NotificationKind(int id, string name) : base(id, name) { }
}
