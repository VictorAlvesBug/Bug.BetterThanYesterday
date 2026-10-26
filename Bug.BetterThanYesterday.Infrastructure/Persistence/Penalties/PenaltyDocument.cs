using Bug.BetterThanYesterday.Domain.Commons;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Penalties;

public class PenaltyDocument : Document
{
	[BsonRepresentation(BsonType.String)]
	public Guid ChallengeId { get; set; }

	[BsonRepresentation(BsonType.String)]
	public Guid UserId { get; set; }

	public DateTime Date { get; set; }

	public decimal Amount { get; set; }

	public string Status { get; set; } = string.Empty;

	public string Reason { get; set; } = string.Empty;

	public DateTime? PaidAt { get; set; }

	public string? PaymentMethod { get; set; }

	public string? ExternalPaymentId { get; set; }

	[BsonRepresentation(BsonType.String)]
	public Guid? ConfirmedByUserId { get; set; }
}
