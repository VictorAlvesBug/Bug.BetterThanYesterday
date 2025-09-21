using Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;
using Bug.BetterThanYesterday.Domain.Plans.Entities;

namespace Bug.BetterThanYesterday.Application.Plans;

internal static class PlanMapper
{
	public static PlanModel ToModel(this Plan plan) => new()
	{
		Id = plan.Id,
		HabitId = plan.HabitId,
		Description = plan.Description,
		StartsAt = plan.StartsAt.ToDateTime(TimeOnly.MinValue),
		EndsAt = plan.EndsAt.ToDateTime(TimeOnly.MinValue),
		StatusId = plan.Status.Id,
		StatusName = plan.Status.Name,
		TypeId = plan.Type.Id,
		TypeName = plan.Type.Name,
		CreatedAt = plan.CreatedAt.ToDateTime(TimeOnly.MinValue),
	};
}
