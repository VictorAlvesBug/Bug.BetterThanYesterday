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
	public async Task<List<Penalty>> ListByPlanIdAsync(Guid planId)
	{
		return (await _collection.FindAsync(doc => doc.PlanId == planId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}

	public async Task<List<Penalty>> ListByPlanIdAndUserIdAsync(Guid planId, Guid userId)
	{
		return (await _collection.FindAsync(doc =>
				doc.PlanId == planId && doc.UserId == userId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}

	public async Task<Penalty?> GetByPlanIdAndUserIdAndDateAsync(Guid planId, Guid userId, DateOnly date)
	{
		var document = (await _collection.FindAsync(doc =>
				doc.PlanId == planId
				&& doc.UserId == userId
				&& doc.Date == date.ToDateTime(TimeOnly.MinValue)))
			.FirstOrDefault();

		return document is null ? null : mapper.ToDomain(document);
	}

	public async Task<decimal> SumPendingAmountByPlanIdAndUserIdAsync(Guid planId, Guid userId)
	{
		var penalties = await ListByPlanIdAndUserIdAsync(planId, userId);
		return penalties
			.Where(p => p.Status == PenaltyStatus.Pending)
			.Sum(p => p.Amount);
	}

	public async Task<bool> HasPendingPenaltiesAsync(Guid planId, Guid userId)
	{
		var count = await _collection.CountDocumentsAsync(doc =>
			doc.PlanId == planId
			&& doc.UserId == userId
			&& doc.Status == PenaltyStatus.Pending.Name);

		return count > 0;
	}
}
