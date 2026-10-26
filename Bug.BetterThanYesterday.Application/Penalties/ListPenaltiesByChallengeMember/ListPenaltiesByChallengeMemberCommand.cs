using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Penalties.ListPenaltiesByChallengeMember;

public sealed class ListPenaltiesByChallengeMemberCommand : ICommand
{
	public ListPenaltiesByChallengeMemberCommand(Guid challengeId, Guid userId)
	{
		ChallengeId = challengeId;
		UserId = userId;
	}

	public Guid ChallengeId { get; }
	public Guid UserId { get; }

	public void Validate()
	{
		if (ChallengeId == Guid.Empty)
			throw new ArgumentException("ChallengeId is required", nameof(ChallengeId));

		if (UserId == Guid.Empty)
			throw new ArgumentException("UserId is required", nameof(UserId));
	}
}
