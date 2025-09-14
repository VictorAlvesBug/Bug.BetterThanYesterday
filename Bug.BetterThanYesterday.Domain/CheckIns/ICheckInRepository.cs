using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Domain.CheckIns
{
	public interface ICheckInRepository : IRepository<CheckIn>
	{
		Task<List<CheckIn>> ListByPlanIdAsync(string planId);
		Task<List<CheckIn>> ListByPlanIdAndUserIdAsync(string planId, string userId);
	}
}
