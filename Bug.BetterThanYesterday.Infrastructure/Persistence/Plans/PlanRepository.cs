using Bug.BetterThanYesterday.Domain.Configurations;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;
using MongoDB.Driver;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Plans
{
	public class PlanRepository(IDatabaseConfig databaseConfig)
		: Repository<Plan>(databaseConfig, "plans"), IPlanRepository
	{
		public async Task<List<Plan>> ListByHabitIdAsync(string habitId)
		{
			return (await _entities.FindAsync(plan => plan.HabitId == habitId)).ToList();
		}
	}
}
