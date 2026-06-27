using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Bug.BetterThanYesterday.Domain.Users.Entities;

namespace Bug.BetterThanYesterday.Application.Plans;

internal static class PlanMapper
{
	public static PlanModel ToModel(this Plan plan, Habit habit, User owner) => new()
	{
		Id = plan.Id,
		OwnerId = plan.OwnerId,
		OwnerName = owner.Nickname,
		HabitId = plan.HabitId,
		HabitName = habit.Name,
		Description = plan.Description,
		StartsAt = plan.StartsAt.ToDateTime(TimeOnly.MinValue),
		EndsAt = plan.EndsAt.ToDateTime(TimeOnly.MinValue),
		Status = plan.GetStatus().Name,
		Type = plan.Type.Name,
		DaysOffPerWeek = plan.DaysOffPerWeek,
		PenaltyValue = plan.PenaltyValue,
		CreatedAt = plan.CreatedAt
	};
}
