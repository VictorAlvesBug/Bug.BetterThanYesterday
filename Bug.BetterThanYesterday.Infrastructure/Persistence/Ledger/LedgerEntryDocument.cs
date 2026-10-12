using Bug.BetterThanYesterday.Domain.Commons;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Ledger;

public class LedgerEntryDocument : Document
{
	[BsonRepresentation(BsonType.String)]
	public Guid PlanId { get; set; }

	[BsonRepresentation(BsonType.String)]
	public Guid UserId { get; set; }

	[BsonRepresentation(BsonType.String)]
	public Guid? PenaltyId { get; set; }

	public DateTime? Date { get; set; }

	public decimal Amount { get; set; }

	public string Kind { get; set; } = string.Empty;

	public string Status { get; set; } = string.Empty;

	public string? ExternalPaymentId { get; set; }
}
