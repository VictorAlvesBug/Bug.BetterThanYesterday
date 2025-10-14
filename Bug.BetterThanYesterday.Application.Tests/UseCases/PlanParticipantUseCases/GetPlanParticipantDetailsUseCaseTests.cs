using Bug.BetterThanYesterday.Application.PlanParticipants;
using Bug.BetterThanYesterday.Application.PlanParticipants.GetPlanParticipantDetails;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.PlanParticipantUseCases;

public class GetPlanParticipantDetailsUseCaseTests : BasePlanParticipantUseCaseTests
{
	[Fact]
	public async Task Test_GetPlanParticipantDetailsUseCase_Valid_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetPlanParticipantDetailsUseCase>();
		var firstPlanParticipant = _mock.PlanParticipants[0];
		var plan = _mock.Plans.First(plan => plan.Id == firstPlanParticipant.PlanId);
		var participant = _mock.Users.First(user => user.Id == firstPlanParticipant.UserId);
		var command = new GetPlanParticipantDetailsCommand(
			firstPlanParticipant.PlanId,
			firstPlanParticipant.UserId
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());

		var resultData = Assert.IsType<Result<PlanParticipantDetailsModel>>(result).Data;
		Assert.Equal(firstPlanParticipant.Id, resultData.PlanParticipantId);
		Assert.Equal(firstPlanParticipant.JoinedAt.ToDateTime(TimeOnly.MinValue), resultData.JoinedAt);
		Assert.Equal(firstPlanParticipant.LeftAt?.ToDateTime(TimeOnly.MinValue), resultData.LeftAt);
		Assert.Equal(firstPlanParticipant.Status.Id, resultData.StatusId);
		Assert.Equal(firstPlanParticipant.Status.Name, resultData.StatusName);
		
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
		
		Assert.Equal(participant.Id, resultData.Participant.UserId);
		Assert.Equal(participant.Name, resultData.Participant.Name);
		Assert.Equal(participant.Email.Value, resultData.Participant.Email);
		Assert.Equal(participant.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.Participant.CreatedAt);

		_mock.PlanParticipantRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
	}

	[Fact]
	public async Task Test_GetPlanParticipantDetailsUseCase_NotFoundPlanParticipantId_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetPlanParticipantDetailsUseCase>();
		var firstPlanParticipant = _mock.PlanParticipants[0];
		var command = new GetPlanParticipantDetailsCommand(
			Guid.NewGuid(),
			Guid.NewGuid()
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		_mock.PlanParticipantRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
	}
}
