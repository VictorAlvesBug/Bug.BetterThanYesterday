using Bug.BetterThanYesterday.Application.DayOffs;
using Bug.BetterThanYesterday.Application.Plans.GetPlanRanking;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;
using Bug.BetterThanYesterday.Domain.DayOffs.Entities;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.Plans;

public class GetPlanRankingUseCaseTests
{
	[Fact]
	public void CalculateStreak_CountsValidatedAndDayOffDays()
	{
		var today = new DateOnly(2025, 6, 28);
		var planStart = new DateOnly(2025, 6, 1);

		var validatedDates = new HashSet<DateOnly>
		{
			today,
			today.AddDays(-1),
			today.AddDays(-3),
		};

		var dayOffs = new List<DayOff>
		{
			CreateDayOff(today.AddDays(-2))
		};

		var streak = GetPlanRankingUseCase.CalculateStreak(validatedDates, dayOffs, today, planStart);

		Assert.Equal(4, streak);
	}

	[Fact]
	public void CalculateStreak_SkipsTodayWhenNoActivityYet()
	{
		var today = new DateOnly(2025, 6, 28);
		var planStart = new DateOnly(2025, 6, 1);

		var validatedDates = new HashSet<DateOnly>
		{
			today.AddDays(-1),
			today.AddDays(-2),
		};

		var streak = GetPlanRankingUseCase.CalculateStreak(validatedDates, [], today, planStart);

		Assert.Equal(2, streak);
	}

	[Fact]
	public void ResolveStatus_PendingWithValidationReview_CountsAsValidated()
	{
		var checkIn = CreateCheckIn(new DateOnly(2025, 6, 28), CheckInStatus.Pending);
		checkIn.AddReview(Review.Create(Guid.NewGuid(), CheckInStatus.Validated.Name, DateTime.Now));

		var status = checkIn.ResolveStatus(4, 1);

		Assert.Equal(CheckInStatus.Validated, status);
	}

	[Fact]
	public void ResolveStatus_ClosedWindowWithoutReviews_CountsAsValidated()
	{
		var checkIn = CreateCheckIn(new DateOnly(2025, 6, 20), CheckInStatus.Pending);

		var status = checkIn.ResolveStatus(4, 1);

		Assert.Equal(CheckInStatus.Validated, status);
	}

	[Fact]
	public void ResolveStatus_RejectedByMajority_ExcludedFromValidated()
	{
		var checkIn = CreateCheckIn(new DateOnly(2025, 6, 28), CheckInStatus.Pending);
		checkIn.AddReview(Review.Create(Guid.NewGuid(), CheckInStatus.Rejected.Name, DateTime.Now));
		checkIn.AddReview(Review.Create(Guid.NewGuid(), CheckInStatus.Rejected.Name, DateTime.Now));
		checkIn.AddReview(Review.Create(Guid.NewGuid(), CheckInStatus.Validated.Name, DateTime.Now));

		var status = checkIn.ResolveStatus(4, 1);

		Assert.Equal(CheckInStatus.Rejected, status);
	}

	[Fact]
	public void DayOffMapper_CountsRollingWindow()
	{
		var reference = new DateOnly(2025, 6, 28);
		var dayOffs = new[]
		{
			CreateDayOff(reference),
			CreateDayOff(reference.AddDays(-3)),
			CreateDayOff(reference.AddDays(-8)),
		};

		var count = DayOffMapper.CountDayOffsInRollingWindow(dayOffs, reference);

		Assert.Equal(2, count);
	}

	private static CheckIn CreateCheckIn(DateOnly date, CheckInStatus status)
	{
		var checkIn = CheckIn.CreateNew(
			Guid.NewGuid(),
			Guid.NewGuid(),
			date.ToDateTime(TimeOnly.MinValue),
			1,
			"Title",
			"https://photo.url");

		checkIn.Status = status;
		return checkIn;
	}

	private static DayOff CreateDayOff(DateOnly date) =>
		DayOff.CreateNew(Guid.NewGuid(), Guid.NewGuid(), date.ToDateTime(TimeOnly.MinValue));
}
