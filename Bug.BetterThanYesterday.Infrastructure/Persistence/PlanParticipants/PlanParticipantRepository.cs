using Bug.BetterThanYesterday.Domain.Configurations;
using Bug.BetterThanYesterday.Domain.PlanParticipants;
using Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;
using MongoDB.Driver;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.PlanParticipants
{
	public class PlanParticipantRepository(IDatabaseConfig databaseConfig)
		: Repository<PlanParticipant>(databaseConfig, "plan_participants"), IPlanParticipantRepository
	{
		public async Task<List<PlanParticipant>> ListByPlanIdAsync(string planId)
		{
			return (await _entities.FindAsync(planParticipant => planParticipant.PlanId == planId)).ToList();
		}

		public async Task<List<PlanParticipant>> ListByUserIdAsync(string userId)
		{
			return (await _entities.FindAsync(planParticipant => planParticipant.UserId == userId)).ToList();
		}
	}
}
