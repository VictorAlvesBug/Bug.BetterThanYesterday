using Bug.BetterThanYesterday.Domain.Commons;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Reports;

public class CheckInReportDocument : Document
{
	[BsonRepresentation(BsonType.String)]
	public Guid CheckInId { get; set; }

	[BsonRepresentation(BsonType.String)]
	public Guid PlanId { get; set; }

	[BsonRepresentation(BsonType.String)]
	public Guid ReporterId { get; set; }

	[BsonRepresentation(BsonType.String)]
	public Guid TargetUserId { get; set; }

	public string Reason { get; set; } = string.Empty;
}
