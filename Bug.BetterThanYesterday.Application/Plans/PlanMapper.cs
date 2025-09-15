using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Plans.Entities;

namespace Bug.BetterThanYesterday.Application.Plans;

internal sealed class PlanMapper : IModelMapper<Plan, PlanModel>
{
	public PlanModel ToModel(Plan plan) => new()
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
