using Bug.BetterThanYesterday.Domain.Configurations;
using Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;
using Bug.BetterThanYesterday.Infrastructure.Persistence.PlanParticipants;
using MongoDB.Driver;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Plans;

public class PlanRepository(
	IDatabaseConfig databaseConfig,
	IDocumentMapper<Plan, PlanDocument> mapper)
	: Repository<Plan, PlanDocument>(
		databaseConfig,
		"plans",
		mapper), IPlanRepository
{
	public async Task<List<Plan>> ListByHabitIdAsync(string habitId)
	{
		return (await _collection.FindAsync(plan => plan.HabitId == habitId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}
}
