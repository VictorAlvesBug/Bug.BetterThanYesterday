using Bug.BetterThanYesterday.Domain.Commons;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.CheckIns;

public class CheckInDocument : Document
{
	[BsonRepresentation(BsonType.String)]
	public Guid PlanId { get; set; }

	[BsonRepresentation(BsonType.String)]
	public Guid UserId { get; set; }
	public DateTime Date { get; set; }
	public int Index { get; set; }
	public string Title { get; set; }
	public string PhotoUrl { get; set; }
	public string Status { get; set; }
	public ReviewObject[] Reviews { get; set; }
}
