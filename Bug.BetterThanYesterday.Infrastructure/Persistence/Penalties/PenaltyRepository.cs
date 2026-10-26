using Bug.BetterThanYesterday.Domain.Penalties;
using Bug.BetterThanYesterday.Domain.Penalties.Entities;
using Bug.BetterThanYesterday.Domain.Penalties.ValueObjects;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;
using MongoDB.Driver;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Penalties;

public class PenaltyRepository(
	IMongoCollection<PenaltyDocument> collection,
	IDocumentMapper<Penalty, PenaltyDocument> mapper)
	: Repository<Penalty, PenaltyDocument>(collection, mapper), IPenaltyRepository
{
	public async Task<List<Penalty>> ListByChallengeIdAsync(Guid challengeId)
	{
		return (await _collection.FindAsync(doc => doc.ChallengeId == challengeId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}

	public async Task<List<Penalty>> ListByChallengeIdAndUserIdAsync(Guid challengeId, Guid userId)
	{
		return (await _collection.FindAsync(doc =>
				doc.ChallengeId == challengeId && doc.UserId == userId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}

	public async Task<Penalty?> GetByChallengeIdAndUserIdAndDateAsync(Guid challengeId, Guid userId, DateOnly date)
	{
		var document = (await _collection.FindAsync(doc =>
				doc.ChallengeId == challengeId
				&& doc.UserId == userId
				&& doc.Date == date.ToDateTime(TimeOnly.MinValue)))
			.FirstOrDefault();

		return document is null ? null : mapper.ToDomain(document);
	}

	public async Task<decimal> SumPendingAmountByChallengeIdAndUserIdAsync(Guid challengeId, Guid userId)
	{
		var penalties = await ListByChallengeIdAndUserIdAsync(challengeId, userId);
		return penalties
			.Where(p => p.Status == PenaltyStatus.Pending)
			.Sum(p => p.Amount);
	}

	public async Task<bool> HasPendingPenaltiesAsync(Guid challengeId, Guid userId)
	{
		var count = await _collection.CountDocumentsAsync(doc =>
			doc.ChallengeId == challengeId
			&& doc.UserId == userId
			&& doc.Status == PenaltyStatus.Pending.Name);

		return count > 0;
	}

	public async Task<Penalty?> GetByExternalPaymentIdAsync(string externalPaymentId)
	{
		var document = (await _collection.FindAsync(doc => doc.ExternalPaymentId == externalPaymentId))
			.FirstOrDefault();

		return document is null ? null : mapper.ToDomain(document);
	}
}
