using Moq;
using Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;
using Bug.BetterThanYesterday.Domain.PlanParticipants;
using Bug.BetterThanYesterday.Domain.PlanParticipants.ValueObjects;

namespace Bug.BetterThanYesterday.Application.Tests.Commons;

public static class PlanParticipantRepositoryMockFactory
{
	public static (Mock<IPlanParticipantRepository> repo, List<PlanParticipant> data) Create()
	{
		var anaId = Guid.Parse("57b8652a-81ad-46af-b50b-e1de389250da");
		var bobId = Guid.Parse("814fbb49-66e1-4d51-a69e-bf1eb6d8fc4a");
		
		var workoutFiveTimesAWeekId = Guid.Parse("40c8f170-b8b8-4e41-ac37-816750808650");
		var readingFifteenPagesEverydayId = Guid.Parse("a7f73852-db21-4791-94b0-1bcb55b0b496");
		var studyingReactEveryWeekendId = Guid.Parse("79754103-5278-4ed2-afc5-bad44e97c4f6");

		List<PlanParticipant> planParticipants =
		[
			PlanParticipant.Restore(
				PlanParticipant.GenerateId(workoutFiveTimesAWeekId, anaId),
				workoutFiveTimesAWeekId,
				anaId,
				new DateTime(2020, 01, 01),
				null,
				PlanParticipantStatus.Active.Id
			),
			PlanParticipant.Restore(
				PlanParticipant.GenerateId(readingFifteenPagesEverydayId, anaId),
				readingFifteenPagesEverydayId,
				anaId,
				new DateTime(2020, 01, 01),
				null,
				PlanParticipantStatus.Active.Id
			),
			PlanParticipant.Restore(
				PlanParticipant.GenerateId(readingFifteenPagesEverydayId, bobId),
				readingFifteenPagesEverydayId,
				bobId,
				new DateTime(2020, 01, 01),
				null,
				PlanParticipantStatus.Left.Id
			),
			PlanParticipant.Restore(
				PlanParticipant.GenerateId(studyingReactEveryWeekendId, bobId),
				studyingReactEveryWeekendId,
				bobId,
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
