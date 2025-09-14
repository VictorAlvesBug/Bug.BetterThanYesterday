using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Plans
{
	internal sealed class PlanMapper : IDocumentMapper<Plan, PlanDocument>
	{
		public PlanDocument ToDocument(Plan plan) => new()
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

		public Plan ToDomain(PlanDocument document) => Plan.Restore(
			document.Id,
			document.HabitId,
			document.Description,
			startsAt: document.StartsAt,
			document.EndsAt,
			(PlanStatus) document.Status,
			(PlanType) document.Type,
			document.CreatedAt);
	}
}
