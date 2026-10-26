using Bug.BetterThanYesterday.Application.Ledger;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.Ledger;

public class SettlementCalculatorTests
{
	[Fact]
	public void Split_ChargesTenPercentAndDividesTheRestByValidatedCheckIns()
	{
		var first = Guid.NewGuid();
		var second = Guid.NewGuid();

		var shares = SettlementCalculator.Split(100, [(first, 2), (second, 1)]);

		Assert.Equal(90, SettlementCalculator.DistributableAmount(100));
		Assert.Equal((first, 60m), shares[0]);
		Assert.Equal((second, 30m), shares[1]);
	}

	[Fact]
	public void Split_SkipsFinishersWithoutValidatedCheckIns()
	{
		var idle = Guid.NewGuid();
		var active = Guid.NewGuid();

		var shares = SettlementCalculator.Split(50, [(idle, 0), (active, 2)]);

		Assert.Single(shares);
		Assert.Equal((active, 45m), shares[0]);
	}
}
