using Bug.BetterThanYesterday.Domain.Commons;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Plans;

public class PlanDocument : Document
{
	[BsonRepresentation(BsonType.String)]
	public Guid OwnerId { get; set; }
	
	[BsonRepresentation(BsonType.String)]
	public Guid HabitId { get; set; }
	public string? Description { get; set; }
	public DateTime StartsAt { get; set; }
	public DateTime EndsAt { get; set; }
	public string Type { get; set; }
	public int DaysOffPerWeek { get; set; }
	public decimal PenaltyValue { get; set; }
	public bool IsCancelled { get; set; }
}
