using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Plans;

internal sealed class PlanMapper : IDocumentMapper<Plan, PlanDocument>
{
	public PlanDocument ToDocument(Plan plan) => new()
	{
		Id = plan.Id,
		OwnerId = plan.OwnerId,
		HabitId = plan.HabitId,
		Description = plan.Description,
		StartsAt = plan.StartsAt.ToDateTime(TimeOnly.MinValue),
		EndsAt = plan.EndsAt.ToDateTime(TimeOnly.MinValue),
		Type = plan.Type.Name,
		DaysOffPerWeek = plan.DaysOffPerWeek,
		PenaltyValue = plan.PenaltyValue,
		IsCancelled = plan.IsCancelled,
		CreatedAt = plan.CreatedAt,
	};

	public Plan ToDomain(PlanDocument document) => Plan.Restore(
		document.Id,
		document.OwnerId,
		document.HabitId,
		document.Description,
		document.StartsAt,
		document.EndsAt,
		document.Type,
		document.DaysOffPerWeek,
		document.PenaltyValue,
		document.IsCancelled,
		document.CreatedAt);
}
