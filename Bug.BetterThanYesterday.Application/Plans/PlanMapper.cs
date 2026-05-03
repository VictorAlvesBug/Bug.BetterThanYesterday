using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.Plans.Entities;

namespace Bug.BetterThanYesterday.Application.Plans;

internal static class PlanMapper
{
	public static PlanModel ToModel(this Plan plan, Habit habit) => new()
	{
		PlanId = plan.Id,
		HabitId = plan.HabitId,
		HabitName = habit.Name,
		Description = plan.Description,
		StartsAt = plan.StartsAt.ToDateTime(TimeOnly.MinValue),
		EndsAt = plan.EndsAt.ToDateTime(TimeOnly.MinValue),
		StatusId = plan.Status.Id,
		StatusName = plan.Status.Name,
		TypeId = plan.Type.Id,
		TypeName = plan.Type.Name,
		CreatedAt = plan.CreatedAt
	};
}
