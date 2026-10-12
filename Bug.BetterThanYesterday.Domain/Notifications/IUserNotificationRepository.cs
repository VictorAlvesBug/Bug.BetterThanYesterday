using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Notifications.Entities;

namespace Bug.BetterThanYesterday.Domain.Notifications;

public interface IUserNotificationRepository : IRepository<UserNotification>
{
	Task<List<UserNotification>> ListByUserIdAsync(Guid userId);
}
