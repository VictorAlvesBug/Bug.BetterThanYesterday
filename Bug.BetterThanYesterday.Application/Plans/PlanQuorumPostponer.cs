using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;

namespace Bug.BetterThanYesterday.Application.Plans;

public static class PlanQuorumPostponer
{
	public static async Task PostponeWhileUnderQuorumAsync(Plan plan, int memberCount, IPlanRepository planRepository)
	{
		if (plan.GetStatus() != PlanStatus.Running || memberCount >= 2)
			return;

		var today = DateOnly.FromDateTime(DateTime.Today);
		var shift = today.DayNumber - plan.StartsAt.DayNumber + 1;
		if (shift < 1)
			return;

		plan.UpdatePeriod(plan.StartsAt.AddDays(shift), plan.EndsAt.AddDays(shift), memberCount);
		await planRepository.UpdateAsync(plan);
	}
}
