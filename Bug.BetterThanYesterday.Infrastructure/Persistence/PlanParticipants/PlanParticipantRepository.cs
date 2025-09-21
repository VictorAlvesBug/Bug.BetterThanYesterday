using Bug.BetterThanYesterday.Domain.PlanParticipants;
using Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;
using MongoDB.Driver;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.PlanParticipants;

public class PlanParticipantRepository(
	IMongoCollection<PlanParticipantDocument> collection,
	IDocumentMapper<PlanParticipant, PlanParticipantDocument> mapper)
	: Repository<PlanParticipant, PlanParticipantDocument>(
		collection,
		mapper), IPlanParticipantRepository
{
	public async Task<List<PlanParticipant>> ListByPlanIdAsync(Guid planId)
	{
		return (await _collection.FindAsync(planParticipant => planParticipant.PlanId == planId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}

	public async Task<List<PlanParticipant>> ListByUserIdAsync(Guid userId)
	{
		return (await _collection.FindAsync(planParticipant => planParticipant.UserId == userId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}
}
