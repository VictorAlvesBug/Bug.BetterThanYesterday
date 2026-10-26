using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Penalties.ListPenaltiesByChallengeId;

public sealed class ListPenaltiesByChallengeIdCommand : ICommand
{
	public ListPenaltiesByChallengeIdCommand(Guid challengeId)
	{
		ChallengeId = challengeId;
	}

	public Guid ChallengeId { get; }

	public void Validate()
	{
		if (ChallengeId == Guid.Empty)
			throw new ArgumentException("ChallengeId is required", nameof(ChallengeId));
	}
}
