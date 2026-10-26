using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Extensions;

namespace Bug.BetterThanYesterday.Domain.ChallengeMembers.ValueObjects;

public sealed class ChallengeMemberStatus : Enumeration<ChallengeMemberStatus>
{
	public static readonly ChallengeMemberStatus Active = new(1, nameof(Active));
	public static readonly ChallengeMemberStatus Blocked = new(2, nameof(Blocked));
	private ChallengeMemberStatus(int id, string name) : base(id, name) { }
}
