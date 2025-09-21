using Bug.BetterThanYesterday.Domain.Commons;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Plans;

public class PlanDocument : Document
{
	[BsonRepresentation(BsonType.String)]
	public Guid HabitId { get; set; }
	public string? Description { get; set; }
	public DateTime StartsAt { get; set; }
	public DateTime EndsAt { get; set; }
	public int StatusId { get; set; }
	public int TypeId { get; set; }
	public DateTime CreatedAt { get; set; }
}
