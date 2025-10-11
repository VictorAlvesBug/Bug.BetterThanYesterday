using Bug.BetterThanYesterday.Domain.CheckIns.Entities;

namespace Bug.BetterThanYesterday.Application.CheckIns;

internal static class CheckInMapper
{
	public static CheckInModel ToModel(this CheckIn checkIn) => new()
	{
		CheckInId = checkIn.Id,
		UserId = checkIn.UserId,
		PlanId = checkIn.PlanId,
		Date = checkIn.Date.ToDateTime(TimeOnly.MinValue),
		Title = checkIn.Title,
		Description = checkIn.Description
	};
}
