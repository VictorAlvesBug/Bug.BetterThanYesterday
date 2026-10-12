using Bug.BetterThanYesterday.Domain.Notifications.Entities;
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
			document.CreatedAt);
}
