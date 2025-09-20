using Bug.BetterThanYesterday.Domain.CheckIns.Entities;

namespace Bug.BetterThanYesterday.Application.CheckIns;

internal static class CheckInMapper
{
	public static CheckInModel ToModel(this CheckIn checkIn) => new()
	{
		Id = checkIn.Id,
		UserId = checkIn.UserId,
		PlanId = checkIn.PlanId,
		Date = checkIn.Date,
		Title = checkIn.Title,
		Description = checkIn.Description
	};
}
