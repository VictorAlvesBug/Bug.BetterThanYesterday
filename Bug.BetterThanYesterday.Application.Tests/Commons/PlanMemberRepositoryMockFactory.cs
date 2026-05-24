using Moq;
using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.PlanMembers.ValueObjects;
using Bug.BetterThanYesterday.Application.Mocks;

namespace Bug.BetterThanYesterday.Application.Tests.Commons;

public static class PlanMemberRepositoryMockFactory
{
	public static (Mock<IPlanMemberRepository> repo, List<PlanMember> data) Create()
	{
		List<PlanMember> planMembers = MockData.MockPlanMembers;

		var planMemberRepository = new Mock<IPlanMemberRepository>();

		planMemberRepository
			.Setup(repo => repo.AddAsync(It.IsAny<PlanMember>()));

		planMemberRepository
			.Setup(repo => repo.ListAllAsync())
			.ReturnsAsync(planMembers);

		planMemberRepository
			.Setup(repo => repo.UpdateAsync(It.IsAny<PlanMember>()));

		planMemberRepository
			.Setup(repo => repo.DeleteAsync(It.IsAny<PlanMember>()));

		planMemberRepository
			.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
			.ReturnsAsync((Guid planMemberId) => planMembers
				.Find(planMember => planMember.Id == planMemberId));

		planMemberRepository
			.Setup(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()))
			.ReturnsAsync((List<Guid> planMemberIds) => planMembers
				.Where(planMember => planMemberIds.Contains(planMember.Id))
				.ToList());

		planMemberRepository
			.Setup(repo => repo.ListByPlanIdAsync(It.IsAny<Guid>()))
			.ReturnsAsync((Guid planId) => planMembers
				.Where(planMember => planMember.PlanId == planId)
				.ToList());

		planMemberRepository
			.Setup(repo => repo.ListByUserIdAsync(It.IsAny<Guid>()))
			.ReturnsAsync((Guid userId) => planMembers
				.Where(planMember => planMember.UserId == userId)
				.ToList());

		return (planMemberRepository, planMembers);
	}
}
