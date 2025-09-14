using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;

namespace Bug.BetterThanYesterday.Domain.PlanParticipants
{
	public interface IPlanParticipantRepository : IRepository<PlanParticipant>
	{
		Task<List<PlanParticipant>> ListByPlanIdAsync(string planId);
		Task<List<PlanParticipant>> ListByUserIdAsync(string userId);
	}
}
