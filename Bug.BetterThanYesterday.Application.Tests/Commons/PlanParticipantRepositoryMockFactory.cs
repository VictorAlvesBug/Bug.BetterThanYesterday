using Moq;
using Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;
using Bug.BetterThanYesterday.Domain.PlanParticipants;
using Bug.BetterThanYesterday.Domain.PlanParticipants.ValueObjects;

namespace Bug.BetterThanYesterday.Application.Tests.Commons;

public static class PlanParticipantRepositoryMockFactory
{
	public static (Mock<IPlanParticipantRepository> repo, List<PlanParticipant> data) Create()
	{
		List<PlanParticipant> planParticipants =
		[
			PlanParticipant.Restore(
				PlanParticipant.BuildId(PlanRepositoryMockFactory.PublicRunningPlanId1_WithUserId1Active, UserRepositoryMockFactory.UserId1),
				PlanRepositoryMockFactory.PublicRunningPlanId1_WithUserId1Active,
				UserRepositoryMockFactory.UserId1,
				DateTime.Today,
				null,
				PlanParticipantStatus.Active.Id
			),
			PlanParticipant.Restore(
				PlanParticipant.BuildId(PlanRepositoryMockFactory.PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Left, UserRepositoryMockFactory.UserId1),
				PlanRepositoryMockFactory.PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Left,
				UserRepositoryMockFactory.UserId1,
				new DateTime(2020, 01, 01),
				null,
				PlanParticipantStatus.Active.Id
			),
			PlanParticipant.Restore(
				PlanParticipant.BuildId(PlanRepositoryMockFactory.PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Left, UserRepositoryMockFactory.UserId2),
				PlanRepositoryMockFactory.PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Left,
				UserRepositoryMockFactory.UserId2,
				new DateTime(2020, 01, 01),
				null,
				PlanParticipantStatus.Blocked.Id
			),
			PlanParticipant.Restore(
				PlanParticipant.BuildId(PlanRepositoryMockFactory.PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Left, UserRepositoryMockFactory.UserId3),
				PlanRepositoryMockFactory.PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Left,
				UserRepositoryMockFactory.UserId3,
				new DateTime(2020, 01, 01),
				null,
				PlanParticipantStatus.Left.Id
			),
			PlanParticipant.Restore(
				PlanParticipant.BuildId(PlanRepositoryMockFactory.PrivateFinishedPlanId4_WithUserId2LeftAndUserId3Blocked, UserRepositoryMockFactory.UserId2),
				PlanRepositoryMockFactory.PrivateFinishedPlanId4_WithUserId2LeftAndUserId3Blocked,
				UserRepositoryMockFactory.UserId2,
				new DateTime(2020, 01, 01),
				null,
				PlanParticipantStatus.Left.Id
			),
			PlanParticipant.Restore(
				PlanParticipant.BuildId(PlanRepositoryMockFactory.PrivateFinishedPlanId4_WithUserId2LeftAndUserId3Blocked, UserRepositoryMockFactory.UserId3),
				PlanRepositoryMockFactory.PrivateFinishedPlanId4_WithUserId2LeftAndUserId3Blocked,
				UserRepositoryMockFactory.UserId3,
				new DateTime(2020, 01, 01),
				null,
				PlanParticipantStatus.Blocked.Id
			),
			PlanParticipant.Restore(
				PlanParticipant.BuildId(PlanRepositoryMockFactory.PrivateFinishedPlanId5_WithUserId5Active, UserRepositoryMockFactory.UserId5),
				PlanRepositoryMockFactory.PrivateFinishedPlanId5_WithUserId5Active,
				UserRepositoryMockFactory.UserId5,
				new DateTime(2020, 01, 01),
				null,
				PlanParticipantStatus.Active.Id
			),
			PlanParticipant.Restore(
				PlanParticipant.BuildId(PlanRepositoryMockFactory.PublicCancelledPlanId6_WithUserId4BlockedAndUserId5Active, UserRepositoryMockFactory.UserId5),
				PlanRepositoryMockFactory.PublicCancelledPlanId6_WithUserId4BlockedAndUserId5Active,
				UserRepositoryMockFactory.UserId5,
				new DateTime(2020, 01, 01),
				null,
				PlanParticipantStatus.Active.Id
			),
			PlanParticipant.Restore(
				PlanParticipant.BuildId(PlanRepositoryMockFactory.PublicRunningPlanId7_WithUserId3BlockedAndUserId4LeftAndUserId5Active, UserRepositoryMockFactory.UserId3),
				PlanRepositoryMockFactory.PublicRunningPlanId7_WithUserId3BlockedAndUserId4LeftAndUserId5Active,
				UserRepositoryMockFactory.UserId3,
				new DateTime(2020, 01, 01),
				null,
				PlanParticipantStatus.Blocked.Id
			),
			PlanParticipant.Restore(
				PlanParticipant.BuildId(PlanRepositoryMockFactory.PublicRunningPlanId7_WithUserId3BlockedAndUserId4LeftAndUserId5Active, UserRepositoryMockFactory.UserId4),
				PlanRepositoryMockFactory.PublicRunningPlanId7_WithUserId3BlockedAndUserId4LeftAndUserId5Active,
				UserRepositoryMockFactory.UserId4,
				new DateTime(2020, 01, 01),
				new DateTime(2020, 01, 05),
				PlanParticipantStatus.Left.Id
			),
			PlanParticipant.Restore(
				PlanParticipant.BuildId(PlanRepositoryMockFactory.PublicRunningPlanId7_WithUserId3BlockedAndUserId4LeftAndUserId5Active, UserRepositoryMockFactory.UserId5),
				PlanRepositoryMockFactory.PublicRunningPlanId7_WithUserId3BlockedAndUserId4LeftAndUserId5Active,
				UserRepositoryMockFactory.UserId5,
				new DateTime(2020, 01, 01),
				null,
				PlanParticipantStatus.Active.Id
			),
			PlanParticipant.Restore(
				PlanParticipant.BuildId(PlanRepositoryMockFactory.PublicCancelledPlanId6_WithUserId4BlockedAndUserId5Active, UserRepositoryMockFactory.UserId4),
				PlanRepositoryMockFactory.PublicCancelledPlanId6_WithUserId4BlockedAndUserId5Active,
				UserRepositoryMockFactory.UserId4,
				new DateTime(2020, 01, 01),
				null,
				PlanParticipantStatus.Blocked.Id
			),
		];

		var planParticipantRepository = new Mock<IPlanParticipantRepository>();

		planParticipantRepository
			.Setup(repo => repo.AddAsync(It.IsAny<PlanParticipant>()));

		planParticipantRepository
			.Setup(repo => repo.ListAllAsync())
			.ReturnsAsync(planParticipants);

		planParticipantRepository
			.Setup(repo => repo.UpdateAsync(It.IsAny<PlanParticipant>()));

		planParticipantRepository
			.Setup(repo => repo.DeleteAsync(It.IsAny<PlanParticipant>()));

		planParticipantRepository
			.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
			.ReturnsAsync((Guid planParticipantId) => planParticipants
				.Find(planParticipant => planParticipant.Id == planParticipantId));

		planParticipantRepository
			.Setup(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()))
			.ReturnsAsync((List<Guid> planParticipantIds) => planParticipants
				.Where(planParticipant => planParticipantIds.Contains(planParticipant.Id))
				.ToList());

		planParticipantRepository
			.Setup(repo => repo.ListByPlanIdAsync(It.IsAny<Guid>()))
			.ReturnsAsync((Guid planId) => planParticipants
				.Where(planParticipant => planParticipant.PlanId == planId)
				.ToList());

		planParticipantRepository
			.Setup(repo => repo.ListByUserIdAsync(It.IsAny<Guid>()))
			.ReturnsAsync((Guid userId) => planParticipants
				.Where(planParticipant => planParticipant.UserId == userId)
				.ToList());

		return (planParticipantRepository, planParticipants);
	}
}
