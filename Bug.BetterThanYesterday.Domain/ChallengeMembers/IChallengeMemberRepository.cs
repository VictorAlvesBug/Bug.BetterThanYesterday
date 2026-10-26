using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.Entities;

namespace Bug.BetterThanYesterday.Domain.ChallengeMembers;

public interface IChallengeMemberRepository : IRepository<ChallengeMember>
{
	Task<List<ChallengeMember>> ListByChallengeIdAsync(Guid challengeId);
	Task<List<ChallengeMember>> ListByUserIdAsync(Guid userId);
}
