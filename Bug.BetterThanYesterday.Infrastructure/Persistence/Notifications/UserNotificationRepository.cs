using Bug.BetterThanYesterday.Domain.Notifications;
using Bug.BetterThanYesterday.Domain.Notifications.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;
using MongoDB.Driver;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Notifications;

public class UserNotificationRepository(
	IMongoCollection<UserNotificationDocument> collection,
	IDocumentMapper<UserNotification, UserNotificationDocument> mapper)
	: Repository<UserNotification, UserNotificationDocument>(collection, mapper), IUserNotificationRepository
{
	public async Task<List<UserNotification>> ListByUserIdAsync(Guid userId)
	{
		return (await _collection.FindAsync(doc => doc.UserId == userId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}
}
