using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
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
		Status = plan.Status.Id,
		Type = plan.Type.Id,
		CreatedAt = plan.CreatedAt.ToDateTime(TimeOnly.MinValue),
	};

	public Plan ToDomain(PlanDocument document) => Plan.Restore(
		document.Id,
		document.HabitId,
		document.Description,
		document.StartsAt,
		document.EndsAt,
		document.Status,
		document.Type,
		document.CreatedAt);
}
