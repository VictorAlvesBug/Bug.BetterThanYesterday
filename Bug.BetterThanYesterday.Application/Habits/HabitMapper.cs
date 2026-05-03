using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.Plans.Entities;

namespace Bug.BetterThanYesterday.Application.Habits;

public static class HabitMapper
{
	public static HabitModel ToModel(this Habit habit, List<Plan>? plans = null) => new()
	{
		HabitId = habit.Id,
		Name = habit.Name,
		CreatedAt = habit.CreatedAt,
		Plans = plans is null 
			? [] 
			: plans.Select(plan => plan.ToModel(habit)).ToList()
	};
}
