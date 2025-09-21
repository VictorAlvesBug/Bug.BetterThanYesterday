using Bug.BetterThanYesterday.Domain.Habits.Policies;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;

namespace Bug.BetterThanYesterday.Application.Habits.DeleteHabit;

public class HabitDeletionPolicy(IPlanRepository planRepository)
	: IHabitDeletionPolicy
{
	public async Task<bool> CanDeleteAsync(Guid habitId)
	{
		var plans = await planRepository.ListByHabitIdAsync(habitId);
		return !plans.Any(plan => plan.Status != PlanStatus.Cancelled);
	}
}
