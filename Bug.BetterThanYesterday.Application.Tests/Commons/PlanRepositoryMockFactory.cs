using Bug.BetterThanYesterday.Application.Mocks;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Moq;

namespace Bug.BetterThanYesterday.Application.Tests.Commons;

public static class PlanRepositoryMockFactory
{
	public static (Mock<IPlanRepository> repo, List<Plan> data) Create()
	{
		List<Plan> plans = MockData.MockPlans;

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
