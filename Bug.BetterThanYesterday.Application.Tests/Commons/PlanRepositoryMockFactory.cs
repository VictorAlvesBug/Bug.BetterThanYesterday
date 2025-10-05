using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Moq;

namespace Bug.BetterThanYesterday.Application.Tests.Commons;

public static class PlanRepositoryMockFactory
{
	public static (Mock<IPlanRepository> repo, List<Plan> data) Create()
	{
		List<Plan> plans =
		[
			Plan.Restore(
				Guid.Parse("40c8f170-b8b8-4e41-ac37-816750808650"),
				Guid.Parse("0160269d-1e78-4ca2-b100-ee42805b5c1e"),
				"Workout 5 times a week",
				new DateTime(2025, 01, 01),
				new DateTime(2025, 12, 31),
				PlanStatus.Running.Id,
				PlanType.Public.Id,
				new DateTime(2024, 06, 15)
			),
			Plan.Restore(
				Guid.Parse("a7f73852-db21-4791-94b0-1bcb55b0b496"),
				Guid.Parse("f523e101-d4b9-453e-8669-c9e8a6918544"),
				"Reading 15 pages everyday",
				new DateTime(2026, 01, 01),
				new DateTime(2026, 12, 31),
				PlanStatus.NotStarted.Id,
				PlanType.Private.Id,
				new DateTime(2025, 10, 05)
			),
			Plan.Restore(
				Guid.Parse("bea8b9e8-5588-460e-bd5d-ae1c042bc166"),
				Guid.Parse("f8cfc6a0-7304-41bb-985e-a3ce9c955bde"),
				"Studing AWS every weekend",
				new DateTime(2025, 01, 01),
				new DateTime(2025, 12, 31),
				PlanStatus.Cancelled.Id,
				PlanType.Public.Id,
				new DateTime(2020, 10, 05)
			),
			Plan.Restore(
				Guid.Parse("79754103-5278-4ed2-afc5-bad44e97c4f6"),
				Guid.Parse("f8cfc6a0-7304-41bb-985e-a3ce9c955bde"),
				"Studing React every weekend",
				new DateTime(2024, 01, 01),
				new DateTime(2024, 12, 31),
				PlanStatus.Finished.Id,
				PlanType.Private.Id,
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
			.ReturnsAsync((Guid planId) => plans.Find(plan => plan.Id == planId));

		planRepository
			.Setup(repo => repo.ListByHabitIdAsync(It.IsAny<Guid>()))
			.ReturnsAsync((Guid habitId) => plans.Where(plan => plan.HabitId == habitId).ToList());

		return (planRepository, plans);
	}
}
