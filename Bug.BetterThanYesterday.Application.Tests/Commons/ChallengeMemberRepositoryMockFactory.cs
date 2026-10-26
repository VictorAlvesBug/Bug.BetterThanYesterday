using Moq;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.Entities;
using Bug.BetterThanYesterday.Domain.ChallengeMembers;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.ValueObjects;
using Bug.BetterThanYesterday.Application.Mocks;

namespace Bug.BetterThanYesterday.Application.Tests.Commons;

public static class ChallengeMemberRepositoryMockFactory
{
	public static (Mock<IChallengeMemberRepository> repo, List<ChallengeMember> data) Create()
	{
		List<ChallengeMember> challengeMembers = MockData.MockChallengeMembers;

		var challengeMemberRepository = new Mock<IChallengeMemberRepository>();

		challengeMemberRepository
			.Setup(repo => repo.AddAsync(It.IsAny<ChallengeMember>()))
			.Returns(Task.CompletedTask);

		challengeMemberRepository
			.Setup(repo => repo.ListAllAsync())
			.ReturnsAsync(challengeMembers);

		challengeMemberRepository
			.Setup(repo => repo.UpdateAsync(It.IsAny<ChallengeMember>()))
			.Returns(Task.CompletedTask);

		challengeMemberRepository
			.Setup(repo => repo.DeleteAsync(It.IsAny<ChallengeMember>()))
			.Returns(Task.CompletedTask);

		challengeMemberRepository
			.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
			.ReturnsAsync((Guid challengeMemberId) => challengeMembers
				.Find(challengeMember => challengeMember.Id == challengeMemberId));

		challengeMemberRepository
			.Setup(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()))
			.ReturnsAsync((List<Guid> challengeMemberIds) => challengeMembers
				.Where(challengeMember => challengeMemberIds.Contains(challengeMember.Id))
				.ToList());

		challengeMemberRepository
			.Setup(repo => repo.ListByChallengeIdAsync(It.IsAny<Guid>()))
			.ReturnsAsync((Guid challengeId) => challengeMembers
				.Where(challengeMember => challengeMember.ChallengeId == challengeId)
				.ToList());

		challengeMemberRepository
			.Setup(repo => repo.ListByUserIdAsync(It.IsAny<Guid>()))
			.ReturnsAsync((Guid userId) => challengeMembers
				.Where(challengeMember => challengeMember.UserId == userId)
				.ToList());

		return (challengeMemberRepository, challengeMembers);
	}
}
