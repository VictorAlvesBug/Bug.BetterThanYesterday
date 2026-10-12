namespace Bug.BetterThanYesterday.Application.Ledger;

public static class SettlementCalculator
{
	public const decimal AdminFeeRate = 0.10m;

	public static decimal DistributableAmount(decimal pot)
	{
		if (pot <= 0)
			return 0;

		return decimal.Round(pot * (1 - AdminFeeRate), 2, MidpointRounding.AwayFromZero);
	}

	public static IReadOnlyList<(Guid UserId, decimal Amount)> Split(
		decimal pot,
		IReadOnlyList<(Guid UserId, int ValidatedCheckIns)> finishers)
	{
		var eligible = finishers.Where(finisher => finisher.ValidatedCheckIns > 0).ToList();
		var total = eligible.Sum(finisher => finisher.ValidatedCheckIns);
		var distributable = DistributableAmount(pot);

		if (distributable <= 0 || total == 0)
			return [];

		var shares = new List<(Guid UserId, decimal Amount)>();
		decimal allocated = 0;

		for (var index = 0; index < eligible.Count; index++)
		{
			var amount = index == eligible.Count - 1
				? distributable - allocated
				: decimal.Round(distributable * eligible[index].ValidatedCheckIns / total, 2, MidpointRounding.AwayFromZero);

			allocated += amount;

			if (amount > 0)
				shares.Add((eligible[index].UserId, amount));
		}

		return shares;
	}
}
