using Bug.BetterThanYesterday.Application.Penalties;
using Bug.BetterThanYesterday.Domain.Challenges.Entities;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.Penalties;

public class DayOffAvailabilityCalculatorTests
{
	[Fact]
	public void SecondAbsenceInAnySevenDayWindow_IsNotAnAutoRest()
	{
		var monday = new DateOnly(2026, 10, 5);
		var wednesday = monday.AddDays(2);
		var challenge = Challenge.Restore(
			Guid.NewGuid(),
			Guid.NewGuid(),
			Guid.NewGuid(),
			"Treino",
			monday.ToDateTime(TimeOnly.MinValue),
			monday.AddDays(30).ToDateTime(TimeOnly.MinValue),
			nameof(ChallengeType.Public),
			daysOffPerWeek: 1,
			penaltyValue: 10,
			isCancelled: false,
			monday.ToDateTime(TimeOnly.MinValue));

		var checkIns = Array.Empty<Domain.CheckIns.Entities.CheckIn>();
		var penalties = Array.Empty<Domain.Penalties.Entities.Penalty>();

		Assert.True(DayOffAvailabilityCalculator.IsAutoRestDay(monday, monday, challenge, checkIns, penalties));
		Assert.False(DayOffAvailabilityCalculator.IsAutoRestDay(wednesday, monday, challenge, checkIns, penalties));
	}
}
