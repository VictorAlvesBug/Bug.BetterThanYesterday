using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Notifications;

public class NotificationPayloadDocument
{
	public string Type { get; set; } = string.Empty;

	[BsonRepresentation(BsonType.String)]
	public Guid ChallengeId { get; set; }

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
