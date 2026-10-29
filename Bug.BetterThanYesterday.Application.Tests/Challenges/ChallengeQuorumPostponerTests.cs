using Bug.BetterThanYesterday.Application.Challenges;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Challenges.Entities;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.Challenges;

public class ChallengeQuorumPostponerTests
{
	[Fact]
	public async Task UnderstaffedRunningChallenge_ShiftsUntilTheStartIsTomorrow()
	{
		var today = DateOnly.FromDateTime(DateTime.Today);
		var start = today.AddDays(-3);
		var end = today.AddDays(10);
		var challenge = Restore(start, end, isCancelled: false);
		var repository = new Mock<IChallengeRepository>();
		repository.Setup(repo => repo.UpdateAsync(challenge)).Returns(Task.CompletedTask);

		await ChallengeQuorumPostponer.PostponeWhileUnderQuorumAsync(challenge, 1, repository.Object);

		Assert.Equal(today.AddDays(1), challenge.StartsAt);
		Assert.Equal(start.AddDays(4), challenge.StartsAt);
		Assert.Equal(end.AddDays(4), challenge.EndsAt);
		repository.Verify(repo => repo.UpdateAsync(challenge), Times.Once);
	}

	[Fact]
	public async Task ChallengeWithTwoMembers_DoesNotMove()
	{
		var today = DateOnly.FromDateTime(DateTime.Today);
		var start = today.AddDays(-1);
		var end = today.AddDays(10);
		var challenge = Restore(start, end, isCancelled: false);
		var repository = new Mock<IChallengeRepository>();

		await ChallengeQuorumPostponer.PostponeWhileUnderQuorumAsync(challenge, 2, repository.Object);

		Assert.Equal(start, challenge.StartsAt);
		Assert.Equal(end, challenge.EndsAt);
		repository.Verify(repo => repo.UpdateAsync(It.IsAny<Challenge>()), Times.Never);
	}

	[Fact]
	public async Task CancelledOrFinishedChallenge_DoesNotMove()
	{
		var today = DateOnly.FromDateTime(DateTime.Today);
		var cancelled = Restore(today.AddDays(-2), today.AddDays(10), isCancelled: true);
		var finished = Restore(today.AddDays(-10), today.AddDays(-1), isCancelled: false);
		var repository = new Mock<IChallengeRepository>();

		await ChallengeQuorumPostponer.PostponeWhileUnderQuorumAsync(cancelled, 0, repository.Object);
		await ChallengeQuorumPostponer.PostponeWhileUnderQuorumAsync(finished, 0, repository.Object);

		Assert.Equal(today.AddDays(-2), cancelled.StartsAt);
		Assert.Equal(today.AddDays(-10), finished.StartsAt);
		repository.Verify(repo => repo.UpdateAsync(It.IsAny<Challenge>()), Times.Never);
	}

	private static Challenge Restore(DateOnly startsAt, DateOnly endsAt, bool isCancelled) =>
		Challenge.Restore(
			Guid.NewGuid(),
			Guid.NewGuid(),
			Guid.NewGuid(),
			"Treino",
			startsAt.ToDateTime(TimeOnly.MinValue),
			endsAt.ToDateTime(TimeOnly.MinValue),
			nameof(ChallengeType.Public),
			daysOffPerWeek: 1,
			penaltyValue: 10,
			isCancelled,
			startsAt.ToDateTime(TimeOnly.MinValue));
}
