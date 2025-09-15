using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;

namespace Bug.BetterThanYesterday.Application.CheckIns;

internal sealed class CheckInMapper : IModelMapper<CheckIn, CheckInModel>
{
	public CheckInModel ToModel(CheckIn checkIn) => new()
	{
		Id = checkIn.Id,
		UserId = checkIn.UserId,
		PlanId = checkIn.PlanId,
		Date = checkIn.Date,
		Title = checkIn.Title,
		Description = checkIn.Description
	};
}
