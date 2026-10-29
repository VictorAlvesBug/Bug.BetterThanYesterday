namespace Bug.BetterThanYesterday.Application.Penalties;

public interface IChallengeDayClosureProcessor
{
	Task ProcessChallengeAsync(Guid challengeId);
}
