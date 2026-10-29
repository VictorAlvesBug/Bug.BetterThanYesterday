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
		ChallengeId = notification.ChallengeId,
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
			document.ChallengeId,
			document.RelatedUserId,
			document.Day,
			document.Kind,
			document.Message,
			ToPayload(document.Payload, document.Kind, document.ChallengeId),
			document.CreatedAt);

	private static NotificationPayloadDocument ToPayloadDocument(NotificationPayload payload) => payload switch
	{
		CheckInReviewedPayload reviewed => new NotificationPayloadDocument
		{
			Type = nameof(NotificationKind.CheckInReviewed),
			ChallengeId = reviewed.ChallengeId,
			CheckInId = reviewed.CheckInId,
			ReviewerId = reviewed.ReviewerId
		},
		CheckInRejectionRiskPayload risk => new NotificationPayloadDocument
		{
			Type = nameof(NotificationKind.CheckInRejectionRisk),
			ChallengeId = risk.ChallengeId,
			CheckInId = risk.CheckInId
		},
		CheckInMadePayload made => new NotificationPayloadDocument
		{
			Type = nameof(NotificationKind.CheckInMade),
			ChallengeId = made.ChallengeId,
			CheckInId = made.CheckInId,
			AuthorId = made.AuthorId
		},
		CheckInReportedPayload reported => new NotificationPayloadDocument
		{
			Type = nameof(NotificationKind.CheckInReported),
			ChallengeId = reported.ChallengeId,
			CheckInId = reported.CheckInId,
			ReporterId = reported.ReporterId,
			TargetUserId = reported.TargetUserId
		},
		_ => new NotificationPayloadDocument
		{
			Type = payload.GetType().Name.Replace("Payload", string.Empty),
			ChallengeId = payload.ChallengeId
		}
	};

	private static NotificationPayload ToPayload(NotificationPayloadDocument? document, string kind, Guid challengeId)
	{
		var type = document?.Type;
		var resolvedChallengeId = document?.ChallengeId is Guid stored && stored != Guid.Empty ? stored : challengeId;

		return type switch
		{
			nameof(NotificationKind.CheckInReviewed) => new CheckInReviewedPayload
			{
				ChallengeId = resolvedChallengeId,
				CheckInId = document?.CheckInId ?? Guid.Empty,
				ReviewerId = document?.ReviewerId ?? Guid.Empty
			},
			nameof(NotificationKind.CheckInRejectionRisk) => new CheckInRejectionRiskPayload
			{
				ChallengeId = resolvedChallengeId,
				CheckInId = document?.CheckInId ?? Guid.Empty
			},
			nameof(NotificationKind.CheckInMade) => new CheckInMadePayload
			{
				ChallengeId = resolvedChallengeId,
				CheckInId = document?.CheckInId ?? Guid.Empty,
				AuthorId = document?.AuthorId ?? Guid.Empty
			},
			nameof(NotificationKind.CheckInReported) => new CheckInReportedPayload
			{
				ChallengeId = resolvedChallengeId,
				CheckInId = document?.CheckInId ?? Guid.Empty,
				ReporterId = document?.ReporterId ?? Guid.Empty,
				TargetUserId = document?.TargetUserId ?? Guid.Empty
			},
			nameof(NotificationKind.RestAtRisk) => new RestAtRiskPayload { ChallengeId = resolvedChallengeId },
			nameof(NotificationKind.FineAtRisk) => new FineAtRiskPayload { ChallengeId = resolvedChallengeId },
			nameof(NotificationKind.ChallengeCancelledForQuorum) => new ChallengeCancelledForQuorumPayload { ChallengeId = resolvedChallengeId },
			_ => NotificationPayload.ForChallenge(NotificationKind.Get(string.IsNullOrWhiteSpace(type) ? kind : type), resolvedChallengeId)
		};
	}
}
