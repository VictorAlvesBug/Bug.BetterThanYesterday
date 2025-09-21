using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Plans;

internal sealed class PlanMapper : IDocumentMapper<Plan, PlanDocument>
{
	public PlanDocument ToDocument(Plan plan) => new()
	{
		Id = plan.Id,
		HabitId = plan.HabitId,
		Description = plan.Description,
		StartsAt = plan.StartsAt.ToDateTime(TimeOnly.MinValue),
		EndsAt = plan.EndsAt.ToDateTime(TimeOnly.MinValue),
		StatusId = plan.Status.Id,
		TypeId = plan.Type.Id,
		CreatedAt = plan.CreatedAt.ToDateTime(TimeOnly.MinValue),
	};

	public Plan ToDomain(PlanDocument document) => Plan.Restore(
		document.Id,
		document.HabitId,
		document.Description,
		document.StartsAt,
		document.EndsAt,
		document.StatusId,
		document.TypeId,
		document.CreatedAt);
}
