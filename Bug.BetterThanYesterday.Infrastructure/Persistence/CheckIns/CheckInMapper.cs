using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.CheckIns;

internal sealed class CheckInMapper : IDocumentMapper<CheckIn, CheckInDocument>
{
	public CheckInDocument ToDocument(CheckIn checkIn) => new()
	{
		Id = checkIn.Id,
		UserId = checkIn.UserId,
		PlanId = checkIn.PlanId,
		Date = checkIn.Date.ToDateTime(TimeOnly.MinValue),
		Title = checkIn.Title,
		Description = checkIn.Description
	};

	public CheckIn ToDomain(CheckInDocument document) => CheckIn.Restore(
		document.Id,
		document.UserId,
		document.PlanId,
		document.Date,
		document.Title,
		document.Description);
}
