using Bug.BetterThanYesterday.Domain.Plans.Entities;

namespace Bug.BetterThanYesterday.Application.Plans;

internal static class PlanMapper
{
	public static PlanModel ToModel(this Plan plan) => new()
	{
		Id = plan.Id,
		HabitId = plan.HabitId,
		Description = plan.Description,
		StartsAt = plan.StartsAt,
		EndsAt = plan.EndsAt,
		Status = plan.Status,
		Type = plan.Type,
		CreatedAt = plan.CreatedAt,
	};
}
