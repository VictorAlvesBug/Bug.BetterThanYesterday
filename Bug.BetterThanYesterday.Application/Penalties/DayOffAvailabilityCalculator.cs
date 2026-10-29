using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;
using Bug.BetterThanYesterday.Domain.Penalties.Entities;
using Bug.BetterThanYesterday.Domain.Challenges.Entities;

namespace Bug.BetterThanYesterday.Application.Penalties;

public static class DayOffAvailabilityCalculator
{
	// Avalia se o dia foi coberto por um checkin validado
    public static DayCoverage EvaluateDayCoverage(DateOnly day, IReadOnlyList<CheckIn> memberCheckIns)
	{
		var dayCheckIns = memberCheckIns.Where(c => c.CalendarDate == day).ToList();

		if (dayCheckIns.Count == 0)
			return DayCoverage.Missed;

		foreach (var checkIn in dayCheckIns)
		{
			if (checkIn.IsReviewWindowOpen())
				return DayCoverage.Deferred;

			var status = checkIn.ResolveStatus();

			if (status == CheckInStatus.Validated)
				return DayCoverage.Covered;

			if (status == CheckInStatus.Pending)
				return DayCoverage.Deferred;
		}

		return DayCoverage.Missed;
	}

	// Contagem dos dias de folga utilizados nos últimos 6 dias	
	public static int CountAutoRestDaysBeforeDay(
		DateOnly day,
		DateOnly memberJoinedAt,
		Challenge challenge,
		IReadOnlyList<CheckIn> memberCheckIns,
		IReadOnlyList<Penalty> memberPenalties)
	{
		// Verifica os 6 dias anteriores ao dia informado
		var windowStart = day.AddDays(-6);
		var count = 0;
	
        for (var cursor = windowStart; cursor < day; cursor = cursor.AddDays(1))
		{
			// Pula dias fora do plano
			if (cursor < challenge.StartsAt || cursor < memberJoinedAt)
				continue;

			// Pula dias com multa
			if (memberPenalties.Any(p => p.Date == cursor))
				continue;

			// Dia contido no período do plano, sem multa e sem checkin validado
            if (EvaluateDayCoverage(cursor, memberCheckIns) == DayCoverage.Missed)
				count++;
		}

		return count;
	}

	public static int GetDaysOffAvailable(
		Challenge challenge,
		DateOnly memberJoinedAt,
		DateOnly referenceDate,
		IReadOnlyList<CheckIn> memberCheckIns,
		IReadOnlyList<Penalty> memberPenalties)
	{
		var autoRestUsed = CountAutoRestDaysBeforeDay(
			referenceDate,
			memberJoinedAt,
			challenge,
			memberCheckIns,
			memberPenalties);

		return Math.Max(0, challenge.DaysOffPerWeek - autoRestUsed);
	}

	public static bool IsAutoRestDay(
		DateOnly day,
		DateOnly memberJoinedAt,
		Challenge challenge,
		IReadOnlyList<CheckIn> memberCheckIns,
		IReadOnlyList<Penalty> memberPenalties)
	{
		if (day < challenge.StartsAt || day < memberJoinedAt)
			return false;

		if (memberPenalties.Any(p => p.Date == day))
			return false;

		if (EvaluateDayCoverage(day, memberCheckIns) != DayCoverage.Missed)
			return false;

		var autoRestsBeforeDay = CountAutoRestDaysBeforeDay(
			day,
			memberJoinedAt,
			challenge,
			memberCheckIns,
			memberPenalties);

		return autoRestsBeforeDay < challenge.DaysOffPerWeek;
	}
}
