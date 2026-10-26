using System.Text.Json.Serialization;

namespace Bug.BetterThanYesterday.Domain.Notifications.ValueObjects;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(CheckInReviewedPayload), "CheckInReviewed")]
[JsonDerivedType(typeof(CheckInRejectionRiskPayload), "CheckInRejectionRisk")]
[JsonDerivedType(typeof(CheckInMadePayload), "CheckInMade")]
[JsonDerivedType(typeof(CheckInReportedPayload), "CheckInReported")]
[JsonDerivedType(typeof(RestAtRiskPayload), "RestAtRisk")]
[JsonDerivedType(typeof(FineAtRiskPayload), "FineAtRisk")]
[JsonDerivedType(typeof(ChallengeStartsTodayPayload), "ChallengeStartsToday")]
[JsonDerivedType(typeof(ChallengeCancelledForQuorumPayload), "ChallengeCancelledForQuorum")]
public abstract class NotificationPayload
{
	public Guid ChallengeId { get; init; }

	public static NotificationPayload ForChallenge(NotificationKind kind, Guid challengeId)
	{
		if (kind == NotificationKind.RestAtRisk)
			return new RestAtRiskPayload { ChallengeId = challengeId };

		if (kind == NotificationKind.FineAtRisk)
			return new FineAtRiskPayload { ChallengeId = challengeId };

		if (kind == NotificationKind.ChallengeStartsToday)
			return new ChallengeStartsTodayPayload { ChallengeId = challengeId };

		if (kind == NotificationKind.ChallengeCancelledForQuorum)
			return new ChallengeCancelledForQuorumPayload { ChallengeId = challengeId };

		return new ChallengeStartsTodayPayload { ChallengeId = challengeId };
	}

	public Guid? ReadCheckInId() => this switch
	{
		CheckInReviewedPayload payload => payload.CheckInId,
		CheckInRejectionRiskPayload payload => payload.CheckInId,
		CheckInMadePayload payload => payload.CheckInId,
		CheckInReportedPayload payload => payload.CheckInId,
		_ => null
	};
}

public sealed class CheckInReviewedPayload : NotificationPayload
{
	public Guid CheckInId { get; init; }
	public Guid ReviewerId { get; init; }
}

public sealed class CheckInRejectionRiskPayload : NotificationPayload
{
	public Guid CheckInId { get; init; }
}

public sealed class CheckInMadePayload : NotificationPayload
{
	public Guid CheckInId { get; init; }
	public Guid AuthorId { get; init; }
}

public sealed class CheckInReportedPayload : NotificationPayload
{
	public Guid CheckInId { get; init; }
	public Guid ReporterId { get; init; }
	public Guid TargetUserId { get; init; }
}

public sealed class RestAtRiskPayload : NotificationPayload;

public sealed class FineAtRiskPayload : NotificationPayload;

public sealed class ChallengeStartsTodayPayload : NotificationPayload;

public sealed class ChallengeCancelledForQuorumPayload : NotificationPayload;
