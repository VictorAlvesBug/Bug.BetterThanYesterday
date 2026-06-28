using Bug.BetterThanYesterday.Application.DayOffs.Models;
using Bug.BetterThanYesterday.Domain.DayOffs.Entities;
using Bug.BetterThanYesterday.Domain.Plans.Entities;

namespace Bug.BetterThanYesterday.Application.DayOffs;

public static class DayOffMapper
{
	public static DayOffModel ToModel(this DayOff dayOff) => new()
	{
		Id = dayOff.Id,
		PlanId = dayOff.PlanId,
		UserId = dayOff.UserId,
		Date = dayOff.Date.ToDateTime(TimeOnly.MinValue),
		CreatedAt = dayOff.CreatedAt
	};

	public static int CountDayOffsInRollingWindow(IEnumerable<DayOff> dayOffs, DateOnly referenceDate)
	{
		var windowStart = referenceDate.AddDays(-6);
		return dayOffs.Count(d => d.Date >= windowStart && d.Date <= referenceDate);
	}

	public static int GetDaysOffAvailable(Plan plan, IEnumerable<DayOff> userDayOffs, DateOnly referenceDate)
	{
		var used = CountDayOffsInRollingWindow(userDayOffs, referenceDate);
		return Math.Max(0, plan.DaysOffPerWeek - used);
	}
}
