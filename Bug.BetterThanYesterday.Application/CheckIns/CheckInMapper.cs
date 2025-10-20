using Bug.BetterThanYesterday.Domain.CheckIns.Entities;

namespace Bug.BetterThanYesterday.Application.CheckIns;

internal static class CheckInMapper
{
	public static CheckInModel ToModel(this CheckIn checkIn) => new()
	{
		CheckInId = checkIn.Id,
		PlanId = checkIn.PlanId,
		UserId = checkIn.UserId,
		Date = checkIn.Date.ToDateTime(TimeOnly.MinValue),
		Index = checkIn.Index,
		Title = checkIn.Title,
		Description = checkIn.Description
	};
}
