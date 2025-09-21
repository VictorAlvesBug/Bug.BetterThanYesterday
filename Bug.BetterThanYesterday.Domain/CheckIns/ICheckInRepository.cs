using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Domain.CheckIns;

public interface ICheckInRepository : IRepository<CheckIn>
{
	Task<List<CheckIn>> ListByPlanIdAsync(Guid planId);
	Task<List<CheckIn>> ListByPlanIdAndUserIdAsync(Guid planId, Guid userId);
}
