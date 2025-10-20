using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.CheckIns;

internal sealed class CheckInMapper : IDocumentMapper<CheckIn, CheckInDocument>
{
	public CheckInDocument ToDocument(CheckIn checkIn) => new()
	{
		Id = checkIn.Id,
		PlanId = checkIn.PlanId,
		UserId = checkIn.UserId,
		Date = checkIn.Date.ToDateTime(TimeOnly.MinValue),
		Index = checkIn.Index,
		Title = checkIn.Title,
		Description = checkIn.Description
	};

	public CheckIn ToDomain(CheckInDocument document) => CheckIn.Restore(
		document.Id,
		document.PlanId,
		document.UserId,
		document.Date,
		document.Index,
		document.Title,
		document.Description);
}
