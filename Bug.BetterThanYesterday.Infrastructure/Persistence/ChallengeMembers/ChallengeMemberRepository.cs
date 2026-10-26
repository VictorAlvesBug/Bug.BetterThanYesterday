using Bug.BetterThanYesterday.Domain.ChallengeMembers;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;
using MongoDB.Driver;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.ChallengeMembers;

public class ChallengeMemberRepository(
	IMongoCollection<ChallengeMemberDocument> collection,
	IDocumentMapper<ChallengeMember, ChallengeMemberDocument> mapper)
	: Repository<ChallengeMember, ChallengeMemberDocument>(
		collection,
		mapper), IChallengeMemberRepository
{
	public async Task<List<ChallengeMember>> ListByChallengeIdAsync(Guid challengeId)
	{
		return (await _collection.FindAsync(challengeMemberDoc => challengeMemberDoc.ChallengeId == challengeId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}

	public async Task<List<ChallengeMember>> ListByUserIdAsync(Guid userId)
	{
		return (await _collection.FindAsync(challengeMemberDoc => challengeMemberDoc.UserId == userId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}
}
