using Bug.BetterThanYesterday.Domain.DayOffs;
using Bug.BetterThanYesterday.Domain.DayOffs.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;
using MongoDB.Driver;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.DayOffs;

public class DayOffRepository(
	IMongoCollection<DayOffDocument> collection,
	IDocumentMapper<DayOff, DayOffDocument> mapper)
	: Repository<DayOff, DayOffDocument>(collection, mapper), IDayOffRepository
{
	public async Task<List<DayOff>> ListByPlanIdAsync(Guid planId)
	{
		return (await _collection.FindAsync(doc => doc.PlanId == planId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}

	public async Task<List<DayOff>> ListByPlanIdAndUserIdAsync(Guid planId, Guid userId)
	{
		return (await _collection.FindAsync(doc =>
				doc.PlanId == planId && doc.UserId == userId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}

	public async Task<DayOff?> GetByPlanIdAndUserIdAndDateAsync(Guid planId, Guid userId, DateOnly date)
	{
		var document = (await _collection.FindAsync(doc =>
				doc.PlanId == planId
				&& doc.UserId == userId
				&& doc.Date == date.ToDateTime(TimeOnly.MinValue)))
			.FirstOrDefault();

		return document is null ? null : mapper.ToDomain(document);
	}
}
