using Bug.BetterThanYesterday.Domain.Commons;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Notifications;

public class NotificationPayloadDocument
{
	public string Type { get; set; } = string.Empty;

	[BsonRepresentation(BsonType.String)]
	public Guid PlanId { get; set; }

	[BsonRepresentation(BsonType.String)]
	public Guid? CheckInId { get; set; }

	[BsonRepresentation(BsonType.String)]
	public Guid? ReviewerId { get; set; }

	[BsonRepresentation(BsonType.String)]
	public Guid? AuthorId { get; set; }

	[BsonRepresentation(BsonType.String)]
	public Guid? ReporterId { get; set; }

	[BsonRepresentation(BsonType.String)]
	public Guid? TargetUserId { get; set; }
}

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

	public NotificationPayloadDocument? Payload { get; set; }
}
