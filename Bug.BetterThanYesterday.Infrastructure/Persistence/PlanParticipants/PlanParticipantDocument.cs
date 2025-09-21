using Bug.BetterThanYesterday.Domain.Commons;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.PlanParticipants;

public class PlanParticipantDocument : Document
{
	[BsonRepresentation(BsonType.String)]
	public Guid PlanId { get; set; }

	[BsonRepresentation(BsonType.String)]
	public Guid UserId { get; set; }
	public DateTime JoinedAt { get; set; }
	public DateTime? LeftAt { get; set; }
	public int StatusId { get; set; }
}
