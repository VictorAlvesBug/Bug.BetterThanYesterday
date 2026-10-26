using System.Text.Json.Serialization;

namespace Bug.BetterThanYesterday.Domain.Notifications.ValueObjects;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(CheckInReviewedPayload), "CheckInReviewed")]
[JsonDerivedType(typeof(CheckInRejectionRiskPayload), "CheckInRejectionRisk")]
[JsonDerivedType(typeof(CheckInMadePayload), "CheckInMade")]
[JsonDerivedType(typeof(CheckInReportedPayload), "CheckInReported")]
[JsonDerivedType(typeof(RestAtRiskPayload), "RestAtRisk")]
[JsonDerivedType(typeof(FineAtRiskPayload), "FineAtRisk")]
[JsonDerivedType(typeof(PlanStartsTodayPayload), "PlanStartsToday")]
[JsonDerivedType(typeof(PlanCancelledForQuorumPayload), "PlanCancelledForQuorum")]
public abstract class NotificationPayload
{
	public Guid PlanId { get; init; }

	public static NotificationPayload ForPlan(NotificationKind kind, Guid planId)
	{
		if (kind == NotificationKind.RestAtRisk)
			return new RestAtRiskPayload { PlanId = planId };

		if (kind == NotificationKind.FineAtRisk)
			return new FineAtRiskPayload { PlanId = planId };

		if (kind == NotificationKind.PlanStartsToday)
			return new PlanStartsTodayPayload { PlanId = planId };

		if (kind == NotificationKind.PlanCancelledForQuorum)
			return new PlanCancelledForQuorumPayload { PlanId = planId };

		return new PlanStartsTodayPayload { PlanId = planId };
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

public sealed class PlanStartsTodayPayload : NotificationPayload;

public sealed class PlanCancelledForQuorumPayload : NotificationPayload;
