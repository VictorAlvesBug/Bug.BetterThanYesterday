using Bug.BetterThanYesterday.Domain.DayOffs.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.DayOffs;

internal sealed class DayOffMapper : IDocumentMapper<DayOff, DayOffDocument>
{
	public DayOffDocument ToDocument(DayOff dayOff) => new()
	{
		Id = dayOff.Id,
		PlanId = dayOff.PlanId,
		UserId = dayOff.UserId,
		Date = dayOff.Date.ToDateTime(TimeOnly.MinValue),
		CreatedAt = dayOff.CreatedAt
	};

	public DayOff ToDomain(DayOffDocument document) =>
		DayOff.Restore(
			document.Id,
			document.PlanId,
			document.UserId,
			document.Date,
			document.CreatedAt);
}
