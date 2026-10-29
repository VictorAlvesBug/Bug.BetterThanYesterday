namespace Bug.BetterThanYesterday.Application.Ledger;

public interface IChallengeFinancesProcessor
{
	Task ProcessAsync(Guid challengeId);
}
