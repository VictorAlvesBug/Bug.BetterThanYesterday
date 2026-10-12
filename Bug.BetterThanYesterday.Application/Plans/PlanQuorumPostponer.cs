using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;

namespace Bug.BetterThanYesterday.Application.Plans;

public static class PlanQuorumPostponer
{
	public static async Task PostponeWhileUnderQuorumAsync(Plan plan, int memberCount, IPlanRepository planRepository)
	{
		var changed = false;

		while (plan.GetStatus() == PlanStatus.Running && memberCount < 2)
		{
			plan.UpdatePeriod(plan.StartsAt.AddDays(1), plan.EndsAt.AddDays(1), memberCount);
			changed = true;
		}

		if (changed)
			await planRepository.UpdateAsync(plan);
	}
}
