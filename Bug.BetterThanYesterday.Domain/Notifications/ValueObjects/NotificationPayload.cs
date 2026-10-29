using System.Text.Json.Serialization;

namespace Bug.BetterThanYesterday.Domain.Notifications.ValueObjects;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(CheckInReviewedPayload), nameof(NotificationKind.CheckInReviewed))]
[JsonDerivedType(typeof(CheckInRejectionRiskPayload), nameof(NotificationKind.CheckInRejectionRisk))]
[JsonDerivedType(typeof(CheckInMadePayload), nameof(NotificationKind.CheckInMade))]
[JsonDerivedType(typeof(CheckInReportedPayload), nameof(NotificationKind.CheckInReported))]
[JsonDerivedType(typeof(RestAtRiskPayload), nameof(NotificationKind.RestAtRisk))]
[JsonDerivedType(typeof(FineAtRiskPayload), nameof(NotificationKind.FineAtRisk))]
[JsonDerivedType(typeof(ChallengeStartsTodayPayload), nameof(NotificationKind.ChallengeStartsToday))]
[JsonDerivedType(typeof(ChallengeCancelledForQuorumPayload), nameof(NotificationKind.ChallengeCancelledForQuorum))]
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
