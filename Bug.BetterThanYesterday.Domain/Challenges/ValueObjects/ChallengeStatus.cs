using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;

public sealed class ChallengeStatus : Enumeration<ChallengeStatus>
{
	public static readonly ChallengeStatus NotStarted = new(1, nameof(NotStarted));
	public static readonly ChallengeStatus Running = new(2, nameof(Running));
	public static readonly ChallengeStatus Finished = new(3, nameof(Finished));
	public static readonly ChallengeStatus Cancelled = new(4, nameof(Cancelled));

	private ChallengeStatus(int id, string name) : base(id, name) { }
}
