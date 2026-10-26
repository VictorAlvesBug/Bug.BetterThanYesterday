using Bug.BetterThanYesterday.Domain.Commons;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.ChallengeMembers;

public class ChallengeMemberDocument : Document
{
	[BsonRepresentation(BsonType.String)]
	public Guid ChallengeId { get; set; }

	[BsonRepresentation(BsonType.String)]
	public Guid UserId { get; set; }
	public DateTime JoinedAt { get; set; }
	public string Status { get; set; } = string.Empty;
	public bool IsDelinquent { get; set; }
}
