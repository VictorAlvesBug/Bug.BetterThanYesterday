using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;
using MongoDB.Driver;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Plans;

public class PlanRepository(
	IMongoCollection<PlanDocument> collection,
	IDocumentMapper<Plan, PlanDocument> mapper)
	: Repository<Plan, PlanDocument>(
		collection,
		mapper), IPlanRepository
{
	public async Task<List<Plan>> ListByOwnerIdAsync(Guid ownerId)
	{
		return (await _collection.FindAsync(planDoc => planDoc.OwnerId == ownerId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}

	public async Task<List<Plan>> ListByHabitIdAsync(Guid habitId)
	{
		return (await _collection.FindAsync(planDoc => planDoc.HabitId == habitId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}
}
