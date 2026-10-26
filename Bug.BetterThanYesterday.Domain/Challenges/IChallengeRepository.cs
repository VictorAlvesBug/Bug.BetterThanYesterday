using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Challenges.Entities;

namespace Bug.BetterThanYesterday.Domain.Challenges;

public interface IChallengeRepository : IRepository<Challenge>
{
	Task<List<Challenge>> ListByOwnerIdAsync(Guid ownerId);
	Task<List<Challenge>> ListByHabitIdAsync(Guid habitId);
}
