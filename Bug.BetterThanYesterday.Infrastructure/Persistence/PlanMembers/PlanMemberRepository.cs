using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;
using MongoDB.Driver;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.PlanMembers;

public class PlanMemberRepository(
	IMongoCollection<PlanMemberDocument> collection,
	IDocumentMapper<PlanMember, PlanMemberDocument> mapper)
	: Repository<PlanMember, PlanMemberDocument>(
		collection,
		mapper), IPlanMemberRepository
{
	public async Task<List<PlanMember>> ListByPlanIdAsync(Guid planId)
	{
		return (await _collection.FindAsync(planMemberDoc => planMemberDoc.PlanId == planId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}

	public async Task<List<PlanMember>> ListByUserIdAsync(Guid userId)
	{
		return (await _collection.FindAsync(planMemberDoc => planMemberDoc.UserId == userId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}
}
