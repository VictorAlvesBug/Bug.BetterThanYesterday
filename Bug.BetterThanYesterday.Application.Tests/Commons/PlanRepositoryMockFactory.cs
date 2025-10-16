using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Moq;

namespace Bug.BetterThanYesterday.Application.Tests.Commons;

public static class PlanRepositoryMockFactory
{
	public static readonly Guid PublicRunningPlanId1_WithUserId1Active = Guid.Parse("40c8f170-b8b8-4e41-ac37-816750808650");
	public static readonly Guid PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Left = Guid.Parse("a7f73852-db21-4791-94b0-1bcb55b0b496");
	public static readonly Guid PublicCancelledPlanId3 = Guid.Parse("bea8b9e8-5588-460e-bd5d-ae1c042bc166");
	public static readonly Guid PrivateFinishedPlanId4_WithUserId2LeftAndUserId3Blocked = Guid.Parse("79754103-5278-4ed2-afc5-bad44e97c4f6");
	public static readonly Guid PrivateFinishedPlanId5_WithUserId5Active = Guid.Parse("453f7331-6170-4cdd-912f-9ffc83a1ea8d");
	public static readonly Guid PublicCancelledPlanId6_WithUserId4BlockedAndUserId5Active = Guid.Parse("fb4e4d61-d64f-4dba-814b-c5e157776c15");
	public static readonly Guid PublicRunningPlanId7_WithUserId3BlockedAndUserId4LeftAndUserId5Active = Guid.Parse("5f63f6bc-bd97-47e7-b3d1-cb4eb64d9b26");

	public static (Mock<IPlanRepository> repo, List<Plan> data) Create()
	{
		List<Plan> plans =
		[
			Plan.Restore(
				PublicRunningPlanId1_WithUserId1Active,
				HabitRepositoryMockFactory.HabitId1,
				"Workout 5 times a week",
				new DateTime(2025, 01, 01),
				new DateTime(2025, 12, 31),
				PlanStatus.Running.Id,
				PlanType.Public.Id,
				new DateTime(2024, 06, 15)
			),
			Plan.Restore(
				PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Left,
				HabitRepositoryMockFactory.HabitId2,
				"Reading 15 pages everyday",
				new DateTime(2026, 01, 01),
				new DateTime(2026, 12, 31),
				PlanStatus.NotStarted.Id,
				PlanType.Private.Id,
				new DateTime(2025, 10, 05)
			),
			Plan.Restore(
				PublicCancelledPlanId3,
				HabitRepositoryMockFactory.HabitId3,
				"Studying AWS every weekend",
				new DateTime(2025, 01, 01),
				new DateTime(2025, 12, 31),
				PlanStatus.Cancelled.Id,
				PlanType.Public.Id,
				new DateTime(2020, 10, 05)
			),
			Plan.Restore(
				PrivateFinishedPlanId4_WithUserId2LeftAndUserId3Blocked,
				HabitRepositoryMockFactory.HabitId3,
				"Studying React every weekend",
				new DateTime(2024, 01, 01),
				new DateTime(2024, 12, 31),
				PlanStatus.Finished.Id,
				PlanType.Private.Id,
				new DateTime(2021, 10, 05)
			),
			Plan.Restore(
				PrivateFinishedPlanId5_WithUserId5Active,
				HabitRepositoryMockFactory.HabitId3,
				"Studying English every weekend",
				new DateTime(2024, 01, 01),
				new DateTime(2024, 12, 31),
				PlanStatus.Finished.Id,
				PlanType.Private.Id,
				new DateTime(2021, 10, 05)
			),
			Plan.Restore(
				PublicCancelledPlanId6_WithUserId4BlockedAndUserId5Active,
				HabitRepositoryMockFactory.HabitId3,
				"Studying Math every weekend",
				new DateTime(2024, 01, 01),
				new DateTime(2024, 12, 31),
				PlanStatus.Cancelled.Id,
				PlanType.Public.Id,
				new DateTime(2021, 10, 05)
			),
			Plan.Restore(
				PublicRunningPlanId7_WithUserId3BlockedAndUserId4LeftAndUserId5Active,
				HabitRepositoryMockFactory.HabitId3,
				"Studying Anatomy every weekend",
				new DateTime(2025, 01, 01),
				new DateTime(DateTime.Today.Year, 12, 31),
				PlanStatus.Running.Id,
				PlanType.Public.Id,
				new DateTime(2021, 10, 05)
			)
		];

		var planRepository = new Mock<IPlanRepository>();

		planRepository.Setup(repo => repo.AddAsync(It.IsAny<Plan>()));

		planRepository
			.Setup(repo => repo.ListAllAsync())
			.ReturnsAsync(plans);

		planRepository
			.Setup(repo => repo.UpdateAsync(It.IsAny<Plan>()));

		planRepository
			.Setup(repo => repo.DeleteAsync(It.IsAny<Plan>()));

		planRepository
			.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
			.ReturnsAsync((Guid planId) => plans
				.Find(plan => plan.Id == planId));

		planRepository
			.Setup(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()))
			.ReturnsAsync((List<Guid> planIds) => plans
				.Where(plan => planIds.Contains(plan.Id))
				.ToList());

		planRepository
			.Setup(repo => repo.ListByHabitIdAsync(It.IsAny<Guid>()))
			.ReturnsAsync((Guid habitId) => plans
				.Where(plan => plan.HabitId == habitId)
				.ToList());

		return (planRepository, plans);
	}
}
