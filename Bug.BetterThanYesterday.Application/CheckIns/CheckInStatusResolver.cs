using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;

namespace Bug.BetterThanYesterday.Application.CheckIns;

public static class CheckInStatusResolver
{
	public static async Task ConsolidateClosedWindowCheckInsAsync(
		IEnumerable<CheckIn> checkIns,
		ICheckInRepository checkInRepository)
	{
		foreach (var checkIn in checkIns)
		{
			if (checkIn.IsReviewWindowOpen() || checkIn.Status != CheckInStatus.Pending)
				continue;

			checkIn.ConsolidateReviewsIntoStatus();
			await checkInRepository.UpdateAsync(checkIn);
		}
	}
}
