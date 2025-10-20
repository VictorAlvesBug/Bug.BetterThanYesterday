using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Domain.CheckIns;

public interface ICheckInRepository : IRepository<CheckIn>
{
	Task<CheckIn> GetDetailsAsync(Guid planId, Guid userId, DateOnly date, int index = 0);
	Task<List<CheckIn>> ListByPlanIdAsync(Guid planId);
	Task<List<CheckIn>> ListByPlanIdAndUserIdAsync(Guid planId, Guid userId);
	Task<List<CheckIn>> ListByPlanIdAndUserIdAndDateAsync(Guid planId, Guid userId, DateOnly date);
}
