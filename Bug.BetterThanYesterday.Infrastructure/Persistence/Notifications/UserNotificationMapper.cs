using Bug.BetterThanYesterday.Domain.Notifications.Entities;
using Bug.BetterThanYesterday.Domain.Notifications.ValueObjects;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Notifications;

internal sealed class UserNotificationMapper : IDocumentMapper<UserNotification, UserNotificationDocument>
{
	public UserNotificationDocument ToDocument(UserNotification notification) => new()
	{
		Id = notification.Id,
		UserId = notification.UserId,
		PlanId = notification.PlanId,
		RelatedUserId = notification.RelatedUserId,
		Day = notification.Day.ToDateTime(TimeOnly.MinValue),
		Kind = notification.Kind.Name,
		Message = notification.Message,
		Payload = ToPayloadDocument(notification.Payload),
		CreatedAt = notification.CreatedAt
	};

	public UserNotification ToDomain(UserNotificationDocument document) =>
		UserNotification.Restore(
			document.Id,
			document.UserId,
			document.PlanId,
			document.RelatedUserId,
			document.Day,
			document.Kind,
			document.Message,
			ToPayload(document.Payload, document.Kind, document.PlanId),
			document.CreatedAt);

	private static NotificationPayloadDocument ToPayloadDocument(NotificationPayload payload) => payload switch
	{
		CheckInReviewedPayload reviewed => new NotificationPayloadDocument
		{
			Type = "CheckInReviewed",
			PlanId = reviewed.PlanId,
			CheckInId = reviewed.CheckInId,
			ReviewerId = reviewed.ReviewerId
		},
		CheckInRejectionRiskPayload risk => new NotificationPayloadDocument
		{
			Type = "CheckInRejectionRisk",
			PlanId = risk.PlanId,
			CheckInId = risk.CheckInId
		},
		CheckInMadePayload made => new NotificationPayloadDocument
		{
			Type = "CheckInMade",
			PlanId = made.PlanId,
			CheckInId = made.CheckInId,
			AuthorId = made.AuthorId
		},
		CheckInReportedPayload reported => new NotificationPayloadDocument
		{
			Type = "CheckInReported",
			PlanId = reported.PlanId,
			CheckInId = reported.CheckInId,
			ReporterId = reported.ReporterId,
			TargetUserId = reported.TargetUserId
		},
		_ => new NotificationPayloadDocument
		{
			Type = payload.GetType().Name.Replace("Payload", string.Empty),
			PlanId = payload.PlanId
		}
	};

	private static NotificationPayload ToPayload(NotificationPayloadDocument? document, string kind, Guid planId)
	{
		var type = document?.Type;
		var resolvedPlanId = document?.PlanId is Guid stored && stored != Guid.Empty ? stored : planId;

		return type switch
		{
			"CheckInReviewed" => new CheckInReviewedPayload
			{
				PlanId = resolvedPlanId,
				CheckInId = document?.CheckInId ?? Guid.Empty,
				ReviewerId = document?.ReviewerId ?? Guid.Empty
			},
			"CheckInRejectionRisk" => new CheckInRejectionRiskPayload
			{
				PlanId = resolvedPlanId,
				CheckInId = document?.CheckInId ?? Guid.Empty
			},
			"CheckInMade" => new CheckInMadePayload
			{
				PlanId = resolvedPlanId,
				CheckInId = document?.CheckInId ?? Guid.Empty,
				AuthorId = document?.AuthorId ?? Guid.Empty
			},
			"CheckInReported" => new CheckInReportedPayload
			{
				PlanId = resolvedPlanId,
				CheckInId = document?.CheckInId ?? Guid.Empty,
				ReporterId = document?.ReporterId ?? Guid.Empty,
				TargetUserId = document?.TargetUserId ?? Guid.Empty
			},
			"RestAtRisk" => new RestAtRiskPayload { PlanId = resolvedPlanId },
			"FineAtRisk" => new FineAtRiskPayload { PlanId = resolvedPlanId },
			"PlanCancelledForQuorum" => new PlanCancelledForQuorumPayload { PlanId = resolvedPlanId },
			_ => NotificationPayload.ForPlan(NotificationKind.Get(string.IsNullOrWhiteSpace(type) ? kind : type), resolvedPlanId)
		};
	}
}
