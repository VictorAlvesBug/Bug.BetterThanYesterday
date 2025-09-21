using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bug.BetterThanYesterday.Domain.Commons;

public class Document
{
	[BsonRepresentation(BsonType.String)]
	public Guid Id { get; set; }
}
