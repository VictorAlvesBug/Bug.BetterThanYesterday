using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Challenges.GetChallengeRanking;

public class GetChallengeRankingCommand : ICommand
{
	public GetChallengeRankingCommand(Guid challengeId, Guid? userId = null)
	{
		ChallengeId = challengeId;
		UserId = userId;
	}

	public Guid ChallengeId { get; }
	public Guid? UserId { get; }

	public void Validate()
	{
		if (ChallengeId == Guid.Empty)
			throw new ArgumentNullException(nameof(ChallengeId), Messages.EnterChallengeId);
	}
}
