using Bug.BetterThanYesterday.Application.Mocks;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Challenges.Entities;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;
using Moq;

namespace Bug.BetterThanYesterday.Application.Tests.Commons;

public static class ChallengeRepositoryMockFactory
{
	public static (Mock<IChallengeRepository> repo, List<Challenge> data) Create()
	{
		foreach (var source in MockData.MockChallenges)
			source.IsCancelled = false;

		List<Challenge> challenges = MockData.MockChallenges
			.Select(source => Challenge.Restore(
				source.Id,
				source.OwnerId,
				source.HabitId,
				source.Description,
				source.StartsAt.ToDateTime(TimeOnly.MinValue),
				source.EndsAt.ToDateTime(TimeOnly.MinValue),
				source.Type.Name,
				source.DaysOffPerWeek,
				source.PenaltyValue,
				false,
				source.CreatedAt))
			.ToList();

		var challengeRepository = new Mock<IChallengeRepository>();

		challengeRepository.Setup(repo => repo.AddAsync(It.IsAny<Challenge>()));

		challengeRepository
			.Setup(repo => repo.ListAllAsync())
			.ReturnsAsync(challenges);

		challengeRepository
			.Setup(repo => repo.UpdateAsync(It.IsAny<Challenge>()))
			.Returns(Task.CompletedTask);

		challengeRepository
			.Setup(repo => repo.DeleteAsync(It.IsAny<Challenge>()));

		challengeRepository
			.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
			.ReturnsAsync((Guid challengeId) => challenges
				.Find(challenge => challenge.Id == challengeId));

		challengeRepository
			.Setup(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()))
			.ReturnsAsync((List<Guid> challengeIds) => challenges
				.Where(challenge => challengeIds.Contains(challenge.Id))
				.ToList());

		challengeRepository
			.Setup(repo => repo.ListByHabitIdAsync(It.IsAny<Guid>()))
			.ReturnsAsync((Guid habitId) => challenges
				.Where(challenge => challenge.HabitId == habitId)
				.ToList());

		return (challengeRepository, challenges);
	}
}
