using Bug.BetterThanYesterday.Application.PlanParticipants;
using Bug.BetterThanYesterday.Application.PlanParticipants.GetPlanWithParticipantsByPlanId;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.PlanParticipantUseCases;

public class GetPlanWithParticipantsByPlanIdUseCaseTests : BasePlanParticipantUseCaseTests
{
	[Fact]
	public async Task Test_GetPlanWithParticipantsByPlanIdUseCase_Valid_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetPlanWithParticipantsByPlanIdUseCase>();
		var planId = _mock.PlanParticipants.First().PlanId;
		var plan = _mock.Plans.First(plan => plan.Id == planId);
		var participants =
			(
				from
					planParticipant in _mock.PlanParticipants
				where
					planParticipant.PlanId == planId
				join user in _mock.Users 
					on planParticipant.UserId equals user.Id
				select
					user
			).Distinct()
			.ToList();
		var command = new GetPlanWithParticipantsByPlanIdCommand(planId);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());

		var resultData = Assert.IsType<Result<PlanWithParticipantsModel>>(result).Data;
		Assert.Equal(plan.Id, resultData.Plan.PlanId);
		Assert.Equal(plan.HabitId, resultData.Plan.HabitId);
		Assert.Equal(plan.Description, resultData.Plan.Description);
		Assert.Equal(plan.StartsAt.ToDateTime(TimeOnly.MinValue), resultData.Plan.StartsAt);
		Assert.Equal(plan.EndsAt.ToDateTime(TimeOnly.MinValue), resultData.Plan.EndsAt);
		Assert.Equal(plan.Status.Id, resultData.Plan.StatusId);
		Assert.Equal(plan.Status.Name, resultData.Plan.StatusName);
		Assert.Equal(plan.Type.Id, resultData.Plan.TypeId);
		Assert.Equal(plan.Type.Name, resultData.Plan.TypeName);
		Assert.Equal(plan.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.Plan.CreatedAt);
		
		Assert.Equal(participants.Count, resultData.Participants.Count);

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.ListByPlanIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()), Times.Once);
	}

	[Fact]
	public async Task Test_GetPlanWithParticipantsByPlanIdUseCase_PlanWithoutParticipants_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetPlanWithParticipantsByPlanIdUseCase>();
		var studyingAwsPlan = _mock.Plans.Find(plan => plan.Description == "Studying AWS every weekend");
		var command = new GetPlanWithParticipantsByPlanIdCommand(studyingAwsPlan.Id);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());

		var resultData = Assert.IsType<Result<PlanWithParticipantsModel>>(result).Data;
		Assert.Equal(studyingAwsPlan.Id, resultData.Plan.PlanId);
		Assert.Equal(studyingAwsPlan.HabitId, resultData.Plan.HabitId);
		Assert.Equal(studyingAwsPlan.Description, resultData.Plan.Description);
		Assert.Equal(studyingAwsPlan.StartsAt.ToDateTime(TimeOnly.MinValue), resultData.Plan.StartsAt);
		Assert.Equal(studyingAwsPlan.EndsAt.ToDateTime(TimeOnly.MinValue), resultData.Plan.EndsAt);
		Assert.Equal(studyingAwsPlan.Status.Id, resultData.Plan.StatusId);
		Assert.Equal(studyingAwsPlan.Status.Name, resultData.Plan.StatusName);
		Assert.Equal(studyingAwsPlan.Type.Id, resultData.Plan.TypeId);
		Assert.Equal(studyingAwsPlan.Type.Name, resultData.Plan.TypeName);
		Assert.Equal(studyingAwsPlan.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.Plan.CreatedAt);
		
		Assert.Empty(resultData.Participants);

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.ListByPlanIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()), Times.Never);
	}

	[Fact]
	public async Task Test_GetPlanWithParticipantsByPlanIdUseCase_NotFoundPlanId_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetPlanWithParticipantsByPlanIdUseCase>();
		var command = new GetPlanWithParticipantsByPlanIdCommand(Guid.NewGuid());

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.ListByPlanIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.UserRepository.Verify(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()), Times.Never);
	}
}
