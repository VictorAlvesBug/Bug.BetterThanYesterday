using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.Plans;

public class PlanQuorumPostponerTests
{
	[Fact]
	public async Task UnderstaffedRunningPlan_ShiftsUntilTheStartIsTomorrow()
	{
		var today = DateOnly.FromDateTime(DateTime.Today);
		var start = today.AddDays(-3);
		var end = today.AddDays(10);
		var plan = Restore(start, end, isCancelled: false);
		var repository = new Mock<IPlanRepository>();
		repository.Setup(repo => repo.UpdateAsync(plan)).Returns(Task.CompletedTask);

		await PlanQuorumPostponer.PostponeWhileUnderQuorumAsync(plan, 1, repository.Object);

		Assert.Equal(today.AddDays(1), plan.StartsAt);
		Assert.Equal(end.AddDays(4), plan.EndsAt);
		repository.Verify(repo => repo.UpdateAsync(plan), Times.Once);
	}

	[Fact]
	public async Task PlanWithTwoMembers_DoesNotMove()
	{
		var today = DateOnly.FromDateTime(DateTime.Today);
		var start = today.AddDays(-1);
		var end = today.AddDays(10);
		var plan = Restore(start, end, isCancelled: false);
		var repository = new Mock<IPlanRepository>();

		await PlanQuorumPostponer.PostponeWhileUnderQuorumAsync(plan, 2, repository.Object);

		Assert.Equal(start, plan.StartsAt);
		Assert.Equal(end, plan.EndsAt);
		repository.Verify(repo => repo.UpdateAsync(It.IsAny<Plan>()), Times.Never);
	}

	[Fact]
	public async Task CancelledOrFinishedPlan_DoesNotMove()
	{
		var today = DateOnly.FromDateTime(DateTime.Today);
		var cancelled = Restore(today.AddDays(-2), today.AddDays(10), isCancelled: true);
		var finished = Restore(today.AddDays(-10), today.AddDays(-1), isCancelled: false);
		var repository = new Mock<IPlanRepository>();

		await PlanQuorumPostponer.PostponeWhileUnderQuorumAsync(cancelled, 0, repository.Object);
		await PlanQuorumPostponer.PostponeWhileUnderQuorumAsync(finished, 0, repository.Object);

		Assert.Equal(today.AddDays(-2), cancelled.StartsAt);
		Assert.Equal(today.AddDays(-10), finished.StartsAt);
		repository.Verify(repo => repo.UpdateAsync(It.IsAny<Plan>()), Times.Never);
	}

	private static Plan Restore(DateOnly startsAt, DateOnly endsAt, bool isCancelled) =>
		Plan.Restore(
			Guid.NewGuid(),
			Guid.NewGuid(),
			Guid.NewGuid(),
			"Treino",
			startsAt.ToDateTime(TimeOnly.MinValue),
			endsAt.ToDateTime(TimeOnly.MinValue),
			"Public",
			daysOffPerWeek: 1,
			penaltyValue: 10,
			isCancelled,
			startsAt.ToDateTime(TimeOnly.MinValue));
}
