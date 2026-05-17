using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.CheckIns;

    public class ReviewObject
	{
		[BsonRepresentation(BsonType.String)]
		public Guid ReviewerId { get; set; }
		public string Status { get; set; }
		public DateTime Date { get; set; }
	}