using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Challenges.Entities;

namespace Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;

public sealed class ChallengeType : Enumeration<ChallengeType>
{
	public static readonly ChallengeType Public = new(1, nameof(Public));
	public static readonly ChallengeType Private = new(2, nameof(Private));

	private ChallengeType(int id, string name) : base(id, name) { }
}
