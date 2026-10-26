using Bug.BetterThanYesterday.Application.Penalties;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.Penalties;

public class DayOffAvailabilityCalculatorTests
{
	[Fact]
	public void SecondAbsenceInAnySevenDayWindow_IsNotAnAutoRest()
	{
		var monday = new DateOnly(2026, 10, 5);
		var wednesday = monday.AddDays(2);
		var plan = Plan.Restore(
			Guid.NewGuid(),
			Guid.NewGuid(),
			Guid.NewGuid(),
			"Treino",
			monday.ToDateTime(TimeOnly.MinValue),
			monday.AddDays(30).ToDateTime(TimeOnly.MinValue),
			"Public",
			daysOffPerWeek: 1,
			penaltyValue: 10,
			isCancelled: false,
			monday.ToDateTime(TimeOnly.MinValue));

		var checkIns = Array.Empty<Domain.CheckIns.Entities.CheckIn>();
		var penalties = Array.Empty<Domain.Penalties.Entities.Penalty>();

		Assert.True(DayOffAvailabilityCalculator.IsAutoRestDay(monday, monday, plan, checkIns, penalties));
		Assert.False(DayOffAvailabilityCalculator.IsAutoRestDay(wednesday, monday, plan, checkIns, penalties));
	}
}
