using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;

namespace Bug.BetterThanYesterday.Domain.PlanMembers;

public interface IPlanMemberRepository : IRepository<PlanMember>
{
	Task<List<PlanMember>> ListByPlanIdAsync(Guid planId);
	Task<List<PlanMember>> ListByUserIdAsync(Guid userId);
}
