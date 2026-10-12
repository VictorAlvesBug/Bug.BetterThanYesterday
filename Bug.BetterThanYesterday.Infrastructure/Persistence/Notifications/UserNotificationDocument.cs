using Bug.BetterThanYesterday.Domain.Commons;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Notifications;

public class UserNotificationDocument : Document
{
	[BsonRepresentation(BsonType.String)]
	public Guid UserId { get; set; }

	[BsonRepresentation(BsonType.String)]
	public Guid PlanId { get; set; }

	[BsonRepresentation(BsonType.String)]
	public Guid? RelatedUserId { get; set; }

	public DateTime Day { get; set; }

	public string Kind { get; set; } = string.Empty;

	public string Message { get; set; } = string.Empty;
}
