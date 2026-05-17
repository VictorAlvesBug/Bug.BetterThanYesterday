using Moq;
using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.PlanMembers.ValueObjects;

namespace Bug.BetterThanYesterday.Application.Tests.Commons;

public static class PlanMemberRepositoryMockFactory
{
	public static (Mock<IPlanMemberRepository> repo, List<PlanMember> data) Create()
	{
		List<PlanMember> planMembers =
		[
			PlanMember.Restore(
				PlanMember.BuildId(PlanRepositoryMockFactory.PublicRunningPlanId1_WithUserId1Active, UserRepositoryMockFactory.UserId1),
				PlanRepositoryMockFactory.PublicRunningPlanId1_WithUserId1Active,
				UserRepositoryMockFactory.UserId1,
				DateTime.Today,
				null,
				PlanMemberStatus.Active.Name,
				DateTime.Today
			),
			PlanMember.Restore(
				PlanMember.BuildId(PlanRepositoryMockFactory.PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active, UserRepositoryMockFactory.UserId1),
				PlanRepositoryMockFactory.PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active,
				UserRepositoryMockFactory.UserId1,
				new DateTime(2020, 01, 01),
				null,
				PlanMemberStatus.Active.Name,
				DateTime.Today
			),
			PlanMember.Restore(
				PlanMember.BuildId(PlanRepositoryMockFactory.PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active, UserRepositoryMockFactory.UserId2),
				PlanRepositoryMockFactory.PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active,
				UserRepositoryMockFactory.UserId2,
				new DateTime(2020, 01, 01),
				null,
				PlanMemberStatus.Blocked.Name,
				DateTime.Today
			),
			PlanMember.Restore(
				PlanMember.BuildId(PlanRepositoryMockFactory.PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active, UserRepositoryMockFactory.UserId3),
				PlanRepositoryMockFactory.PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active,
				UserRepositoryMockFactory.UserId3,
				new DateTime(2020, 01, 01),
				null,
				PlanMemberStatus.Active.Name,
				DateTime.Today
			),
			PlanMember.Restore(
				PlanMember.BuildId(PlanRepositoryMockFactory.PrivateFinishedPlanId4_WithUserId2ActiveAndUserId3Blocked, UserRepositoryMockFactory.UserId2),
				PlanRepositoryMockFactory.PrivateFinishedPlanId4_WithUserId2ActiveAndUserId3Blocked,
				UserRepositoryMockFactory.UserId2,
				new DateTime(2020, 01, 01),
				null,
				PlanMemberStatus.Active.Name,
				DateTime.Today
			),
			PlanMember.Restore(
				PlanMember.BuildId(PlanRepositoryMockFactory.PrivateFinishedPlanId4_WithUserId2ActiveAndUserId3Blocked, UserRepositoryMockFactory.UserId3),
				PlanRepositoryMockFactory.PrivateFinishedPlanId4_WithUserId2ActiveAndUserId3Blocked,
				UserRepositoryMockFactory.UserId3,
				new DateTime(2020, 01, 01),
				null,
				PlanMemberStatus.Blocked.Name,
				DateTime.Today
			),
			PlanMember.Restore(
				PlanMember.BuildId(PlanRepositoryMockFactory.PrivateFinishedPlanId5_WithUserId5Active, UserRepositoryMockFactory.UserId5),
				PlanRepositoryMockFactory.PrivateFinishedPlanId5_WithUserId5Active,
				UserRepositoryMockFactory.UserId5,
				new DateTime(2020, 01, 01),
				null,
				PlanMemberStatus.Active.Name,
				DateTime.Today
			),
			PlanMember.Restore(
				PlanMember.BuildId(PlanRepositoryMockFactory.PublicCancelledPlanId6_WithUserId4BlockedAndUserId5Active, UserRepositoryMockFactory.UserId5),
				PlanRepositoryMockFactory.PublicCancelledPlanId6_WithUserId4BlockedAndUserId5Active,
				UserRepositoryMockFactory.UserId5,
				new DateTime(2020, 01, 01),
				null,
				PlanMemberStatus.Active.Name,
				DateTime.Today
			),
			PlanMember.Restore(
				PlanMember.BuildId(PlanRepositoryMockFactory.PublicRunningPlanId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active, UserRepositoryMockFactory.UserId3),
				PlanRepositoryMockFactory.PublicRunningPlanId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active,
				UserRepositoryMockFactory.UserId3,
				new DateTime(2020, 01, 01),
				null,
				PlanMemberStatus.Blocked.Name,
				DateTime.Today
			),
			PlanMember.Restore(
				PlanMember.BuildId(PlanRepositoryMockFactory.PublicRunningPlanId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active, UserRepositoryMockFactory.UserId4),
				PlanRepositoryMockFactory.PublicRunningPlanId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active,
				UserRepositoryMockFactory.UserId4,
				new DateTime(2020, 01, 01),
				new DateTime(2020, 01, 05),
				PlanMemberStatus.Active.Name,
				DateTime.Today
			),
			PlanMember.Restore(
				PlanMember.BuildId(PlanRepositoryMockFactory.PublicRunningPlanId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active, UserRepositoryMockFactory.UserId5),
				PlanRepositoryMockFactory.PublicRunningPlanId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active,
				UserRepositoryMockFactory.UserId5,
				new DateTime(2020, 01, 01),
				null,
				PlanMemberStatus.Active.Name,
				DateTime.Today
			),
			PlanMember.Restore(
				PlanMember.BuildId(PlanRepositoryMockFactory.PublicCancelledPlanId6_WithUserId4BlockedAndUserId5Active, UserRepositoryMockFactory.UserId4),
				PlanRepositoryMockFactory.PublicCancelledPlanId6_WithUserId4BlockedAndUserId5Active,
				UserRepositoryMockFactory.UserId4,
				new DateTime(2020, 01, 01),
				null,
				PlanMemberStatus.Blocked.Name,
				DateTime.Today
			),
		];

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
