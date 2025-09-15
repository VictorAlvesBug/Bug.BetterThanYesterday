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
	public async Task<List<Plan>> ListByHabitIdAsync(string habitId)
	{
		return (await _collection.FindAsync(plan => plan.HabitId == habitId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}
}
