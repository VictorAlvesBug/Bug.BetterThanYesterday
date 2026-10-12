using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Notifications.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Domain.Notifications.Entities;

public sealed class UserNotification : Entity
{
	public Guid UserId { get; private set; }
	public Guid PlanId { get; private set; }
	public Guid? RelatedUserId { get; private set; }
	public DateOnly Day { get; private set; }
	public NotificationKind Kind { get; private set; } = null!;
	public string Message { get; private set; } = string.Empty;

	public static UserNotification Create(
		Guid userId,
		Guid planId,
		NotificationKind kind,
		string message,
		DateOnly day,
		Guid? relatedUserId = null)
	{
		if (userId == Guid.Empty)
			throw new ArgumentNullException(nameof(userId), Messages.EnterUserId);

		if (planId == Guid.Empty)
			throw new ArgumentNullException(nameof(planId), Messages.EnterPlanId);

		if (string.IsNullOrWhiteSpace(message))
			throw new ArgumentException(Messages.GenericError, nameof(message));

		return new UserNotification
		{
			Id = Guid.NewGuid(),
			UserId = userId,
			PlanId = planId,
			RelatedUserId = relatedUserId,
			Day = day,
			Kind = kind,
			Message = message.Trim(),
			CreatedAt = DateTime.Now
		};
	}

	public static UserNotification Restore(
		Guid id,
		Guid userId,
		Guid planId,
		Guid? relatedUserId,
		DateTime day,
		string kind,
		string message,
		DateTime createdAt) => new()
	{
		Id = id,
		UserId = userId,
		PlanId = planId,
		RelatedUserId = relatedUserId,
		Day = DateOnly.FromDateTime(day),
		Kind = NotificationKind.Get(kind),
		Message = message,
		CreatedAt = createdAt
	};
}
