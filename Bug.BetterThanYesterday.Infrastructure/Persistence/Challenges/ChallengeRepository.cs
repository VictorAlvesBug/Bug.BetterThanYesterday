using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Challenges.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;
using MongoDB.Driver;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Challenges;

public class ChallengeRepository(
	IMongoCollection<ChallengeDocument> collection,
	IDocumentMapper<Challenge, ChallengeDocument> mapper)
	: Repository<Challenge, ChallengeDocument>(
		collection,
		mapper), IChallengeRepository
{
	public async Task<List<Challenge>> ListByOwnerIdAsync(Guid ownerId)
	{
		return (await _collection.FindAsync(challengeDoc => challengeDoc.OwnerId == ownerId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}

	public async Task<List<Challenge>> ListByHabitIdAsync(Guid habitId)
	{
		return (await _collection.FindAsync(challengeDoc => challengeDoc.HabitId == habitId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}
}
