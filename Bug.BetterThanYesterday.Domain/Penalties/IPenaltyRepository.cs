using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Penalties.Entities;
using Bug.BetterThanYesterday.Domain.Penalties.ValueObjects;

namespace Bug.BetterThanYesterday.Domain.Penalties;

public interface IPenaltyRepository : IRepository<Penalty>
{
	Task<List<Penalty>> ListByPlanIdAsync(Guid planId);
	Task<List<Penalty>> ListByPlanIdAndUserIdAsync(Guid planId, Guid userId);
	Task<Penalty?> GetByPlanIdAndUserIdAndDateAsync(Guid planId, Guid userId, DateOnly date);
	Task<decimal> SumPendingAmountByPlanIdAndUserIdAsync(Guid planId, Guid userId);
	Task<bool> HasPendingPenaltiesAsync(Guid planId, Guid userId);
	Task<Penalty?> GetByExternalPaymentIdAsync(string externalPaymentId);
}
