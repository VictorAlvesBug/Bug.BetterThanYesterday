using Bug.BetterThanYesterday.Domain.Configurations;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.PlanParticipants;
using Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Habits;
using MongoDB.Driver;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.PlanParticipants;

public class PlanParticipantRepository(
	IDatabaseConfig databaseConfig,
	IDocumentMapper<PlanParticipant, PlanParticipantDocument> mapper)
	: Repository<PlanParticipant, PlanParticipantDocument>(
		databaseConfig,
		"plan_participants",
		mapper), IPlanParticipantRepository
{
	public async Task<List<PlanParticipant>> ListByPlanIdAsync(string planId)
	{
		return (await _collection.FindAsync(planParticipant => planParticipant.PlanId == planId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}

	public async Task<List<PlanParticipant>> ListByUserIdAsync(string userId)
	{
		return (await _collection.FindAsync(planParticipant => planParticipant.UserId == userId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}
}
