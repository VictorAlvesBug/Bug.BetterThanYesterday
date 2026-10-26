using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Penalties.Entities;
using Bug.BetterThanYesterday.Domain.Penalties.ValueObjects;

namespace Bug.BetterThanYesterday.Domain.Penalties;

public interface IPenaltyRepository : IRepository<Penalty>
{
	Task<List<Penalty>> ListByChallengeIdAsync(Guid challengeId);
	Task<List<Penalty>> ListByChallengeIdAndUserIdAsync(Guid challengeId, Guid userId);
	Task<Penalty?> GetByChallengeIdAndUserIdAndDateAsync(Guid challengeId, Guid userId, DateOnly date);
	Task<decimal> SumPendingAmountByChallengeIdAndUserIdAsync(Guid challengeId, Guid userId);
	Task<bool> HasPendingPenaltiesAsync(Guid challengeId, Guid userId);
	Task<Penalty?> GetByExternalPaymentIdAsync(string externalPaymentId);
}
