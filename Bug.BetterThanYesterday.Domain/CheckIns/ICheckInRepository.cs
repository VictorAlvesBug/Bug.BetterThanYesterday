using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Domain.CheckIns;

public interface ICheckInRepository : IRepository<CheckIn>
{
	Task<CheckIn> GetDetailsAsync(Guid challengeId, Guid userId, DateOnly date, int index = 0);
	Task<List<CheckIn>> ListByChallengeIdAsync(Guid challengeId);
	Task<List<CheckIn>> ListByChallengeIdAndUserIdAsync(Guid challengeId, Guid userId);
	Task<List<CheckIn>> ListByChallengeIdAndUserIdAndDateAsync(Guid challengeId, Guid userId, DateOnly date);
}
