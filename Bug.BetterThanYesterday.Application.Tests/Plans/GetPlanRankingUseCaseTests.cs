using Bug.BetterThanYesterday.Application.Penalties;
using Bug.BetterThanYesterday.Application.Plans.GetPlanRanking;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;
using Bug.BetterThanYesterday.Domain.Penalties.Entities;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.Plans;

public class GetPlanRankingUseCaseTests
{
	[Fact]
	public void CalculateStreak_CountsValidatedAndAutoRestDaysAndPendingCheckins()
	{
		var today = new DateOnly(2025, 6, 28);
		var planStart = new DateOnly(2025, 6, 1);
		var memberJoinedAt = planStart;
		var plan = CreatePlan(planStart, daysOffPerWeek: 6);

		var validatedDates = new HashSet<DateOnly>
		{
			today,
			today.AddDays(-1),
			today.AddDays(-3),
		};

		var memberCheckIns = BuildValidatedCheckIns(validatedDates);

		var streak = GetPlanRankingUseCase.CalculateStreak(
			validatedDates,
			0,
			memberJoinedAt,
			plan,
			memberCheckIns,
			[],
			today);

		Assert.Equal(4, streak);
	}

	[Fact]
	public void CalculateStreak_SkipsTodayWhenNoActivityYetAndNoPendingCheckins()
	{
		var today = new DateOnly(2025, 6, 28);
		var planStart = new DateOnly(2025, 6, 1);
		var plan = CreatePlan(planStart, daysOffPerWeek: 6);

		var validatedDates = new HashSet<DateOnly>
		{
			today.AddDays(-1),
			today.AddDays(-2),
		};

		var memberCheckIns = BuildValidatedCheckIns(validatedDates);

		var streak = GetPlanRankingUseCase.CalculateStreak(
			validatedDates,
			0,
			planStart,
			plan,
			memberCheckIns,
			[],
			today);

		Assert.Equal(2, streak);
	}

	[Fact]
	public void ResolveStatus_OpenWindowWithValidationReview_ReturnsPending()
	{
		var checkIn = CreateCheckIn(DateTime.Now.AddHours(-1), CheckInStatus.Pending, DateTime.Now.AddHours(-1));
		checkIn.AddReview(Review.Create(Guid.NewGuid(), CheckInStatus.Validated.Name, DateTime.Now));

		var status = checkIn.ResolveStatus();

		Assert.Equal(CheckInStatus.Pending, status);
	}

	[Fact]
	public void ResolveStatus_ClosedWindowWithoutReviews_ReturnsValidated()
	{
		var checkIn = CreateCheckIn(DateTime.Now.AddHours(-25), CheckInStatus.Pending, DateTime.Now.AddHours(-25));

		var status = checkIn.ResolveStatus();

		Assert.Equal(CheckInStatus.Validated, status);
	}

	[Fact]
	public void ResolveStatus_ClosedWindowRejectedByMajority_ReturnsRejected()
	{
		var checkIn = CreateCheckIn(DateTime.Now.AddHours(-25), CheckInStatus.Pending, DateTime.Now.AddHours(-25));
		checkIn.AddReview(Review.Create(Guid.NewGuid(), CheckInStatus.Rejected.Name, DateTime.Now));
		checkIn.AddReview(Review.Create(Guid.NewGuid(), CheckInStatus.Rejected.Name, DateTime.Now));
		checkIn.AddReview(Review.Create(Guid.NewGuid(), CheckInStatus.Validated.Name, DateTime.Now));

		var status = checkIn.ResolveStatus();

		Assert.Equal(CheckInStatus.Rejected, status);
	}

	[Fact]
	public void IsReviewWindowOpen_Within24Hours_ReturnsTrue()
	{
		var checkIn = CreateCheckIn(DateTime.Now.AddHours(-1), CheckInStatus.Pending, DateTime.Now.AddHours(-1));

		Assert.True(checkIn.IsReviewWindowOpen());
	}

	[Fact]
	public void IsReviewWindowOpen_After24Hours_ReturnsFalse()
	{
		var checkIn = CreateCheckIn(DateTime.Now.AddHours(-25), CheckInStatus.Pending, DateTime.Now.AddHours(-25));

		Assert.False(checkIn.IsReviewWindowOpen());
	}

	[Fact]
	public void DayOffAvailabilityCalculator_CountsRollingWindow()
	{
		var reference = new DateOnly(2025, 6, 28);
		var planStart = new DateOnly(2025, 6, 1);
		var plan = CreatePlan(planStart, daysOffPerWeek: 6);
		var memberJoinedAt = planStart;

		var count = DayOffAvailabilityCalculator.CountAutoRestDaysBeforeDay(
			reference,
			memberJoinedAt,
			plan,
			[],
			[]);

		Assert.Equal(7, count);
	}

	private static Plan CreatePlan(DateOnly startsAt, int daysOffPerWeek)
	{
		return Plan.Restore(
			Guid.NewGuid(),
			Guid.NewGuid(),
			Guid.NewGuid(),
			"Description",
			startsAt.ToDateTime(TimeOnly.MinValue),
			startsAt.AddMonths(1).ToDateTime(TimeOnly.MinValue),
			PlanType.Public.Name,
			daysOffPerWeek,
			10m,
			false,
			DateTime.Now);
	}

	private static List<CheckIn> BuildValidatedCheckIns(IEnumerable<DateOnly> dates)
	{
		return dates.Select(date => RestoreCheckIn(
			date.ToDateTime(TimeOnly.MinValue),
			CheckInStatus.Validated,
			createdAt: date.ToDateTime(TimeOnly.MinValue))).ToList();
	}

	private static CheckIn RestoreCheckIn(
		DateTime date,
		CheckInStatus status,
		DateTime createdAt,
		List<Review>? reviews = null)
	{
		return CheckIn.Restore(
			Guid.NewGuid(),
			Guid.NewGuid(),
			Guid.NewGuid(),
			date,
			1,
			"Title",
			"https://photo.url",
			status.Name,
			reviews ?? [],
			createdAt);
	}

	private static CheckIn CreateCheckIn(DateTime date, CheckInStatus status, DateTime? createdAt = null)
	{
		return RestoreCheckIn(date, status, createdAt ?? date);
	}
}
