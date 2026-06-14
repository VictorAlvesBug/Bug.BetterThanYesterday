using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Users.Entities;

namespace Bug.BetterThanYesterday.Application.Habits;

public static class HabitMapper
{
	public static HabitModel ToModel(
		this Habit habit,
		List<Plan>? plans = null,
		List<User>? owners = null) => new()
	{
		Id = habit.Id,
		Name = habit.Name,
		CreatedAt = habit.CreatedAt,
		Plans = plans is null 
			? [] 
			: plans.Select(plan => plan.ToModel(
				habit,
				owners!.First(owner => owner.Id == plan.OwnerId))).ToList()
	};
}
