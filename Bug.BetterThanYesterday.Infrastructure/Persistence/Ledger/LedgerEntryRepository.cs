using Bug.BetterThanYesterday.Domain.Ledger;
using Bug.BetterThanYesterday.Domain.Ledger.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;
using MongoDB.Driver;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Ledger;

public class LedgerEntryRepository(
	IMongoCollection<LedgerEntryDocument> collection,
	IDocumentMapper<LedgerEntry, LedgerEntryDocument> mapper)
	: Repository<LedgerEntry, LedgerEntryDocument>(collection, mapper), ILedgerRepository
{
	public async Task<List<LedgerEntry>> ListByChallengeIdAsync(Guid challengeId)
	{
		return (await _collection.FindAsync(doc => doc.ChallengeId == challengeId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}
}
