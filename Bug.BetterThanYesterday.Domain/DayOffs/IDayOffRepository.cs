using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.DayOffs.Entities;

namespace Bug.BetterThanYesterday.Domain.DayOffs;

public interface IDayOffRepository : IRepository<DayOff>
{
	Task<List<DayOff>> ListByPlanIdAsync(Guid planId);
	Task<List<DayOff>> ListByPlanIdAndUserIdAsync(Guid planId, Guid userId);
	Task<DayOff?> GetByPlanIdAndUserIdAndDateAsync(Guid planId, Guid userId, DateOnly date);
}
