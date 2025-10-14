using Bug.BetterThanYesterday.Application.PlanParticipants;
using Bug.BetterThanYesterday.Application.PlanParticipants.RemoveUserFromPlan;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.PlanParticipantUseCases;

public class RemoveUserFromPlanUseCaseTests : BasePlanParticipantUseCaseTests
{
	/*[Fact]
	public async Task Test_RemoveUserFromPlanUseCase_Valid_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RemoveUserFromPlanUseCase>();
		var firstPlanParticipant = _mock.PlanParticipants[0];
		var command = new RemoveUserFromPlanCommand();

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());

		var resultData = Assert.IsType<Result<PlanParticipantModel>>(result).Data;
		Assert.Equal(firstPlanParticipant.Prop, resultData.Prop);
		Assert.Equal(firstPlanParticipant.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.CreatedAt);

		_mock.PlanRepository.Verify(repo => repo.zzzAsync(It.IsAny<Plan>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.zzzAsync(It.IsAny<PlanParticipant>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.zzzAsync(It.IsAny<PlanParticipant>()), Times.Once);
	}

	[Fact]
	public async Task Test_RemoveUserFromPlanUseCase_NotFoundPlanId_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RemoveUserFromPlanUseCase>();
		var firstPlanParticipant = _mock.PlanParticipants[0];
		var command = new RemoveUserFromPlanCommand();

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		var resultData = Assert.IsType<Result<PlanParticipantModel>>(result).Data;
		Assert.Equal(firstPlanParticipant.Prop, resultData.Prop);
		Assert.Equal(firstPlanParticipant.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.CreatedAt);

		_mock.PlanRepository.Verify(repo => repo.zzzAsync(It.IsAny<Plan>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.zzzAsync(It.IsAny<PlanParticipant>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.zzzAsync(It.IsAny<PlanParticipant>()), Times.Once);
	}

	[Fact]
	public async Task Test_RemoveUserFromPlanUseCase_NotFoundUserId_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RemoveUserFromPlanUseCase>();
		var firstPlanParticipant = _mock.PlanParticipants[0];
		var command = new RemoveUserFromPlanCommand();

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		var resultData = Assert.IsType<Result<PlanParticipantModel>>(result).Data;
		Assert.Equal(firstPlanParticipant.Prop, resultData.Prop);
		Assert.Equal(firstPlanParticipant.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.CreatedAt);

		_mock.PlanRepository.Verify(repo => repo.zzzAsync(It.IsAny<Plan>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.zzzAsync(It.IsAny<PlanParticipant>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.zzzAsync(It.IsAny<PlanParticipant>()), Times.Once);
	}

	[Fact]
	public async Task Test_RemoveUserFromPlanUseCase_UserIsNotInThePlan_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RemoveUserFromPlanUseCase>();
		var firstPlanParticipant = _mock.PlanParticipants[0];
		var command = new RemoveUserFromPlanCommand();

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		var resultData = Assert.IsType<Result<PlanParticipantModel>>(result).Data;
		Assert.Equal(firstPlanParticipant.Prop, resultData.Prop);
		Assert.Equal(firstPlanParticipant.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.CreatedAt);

		_mock.PlanRepository.Verify(repo => repo.zzzAsync(It.IsAny<Plan>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.zzzAsync(It.IsAny<PlanParticipant>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.zzzAsync(It.IsAny<PlanParticipant>()), Times.Once);
	}

	[Fact]
	public async Task Test_RemoveUserFromPlanUseCase_UserCannotBeRemovedFromFinishedPlan_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RemoveUserFromPlanUseCase>();
		var firstPlanParticipant = _mock.PlanParticipants[0];
		var command = new RemoveUserFromPlanCommand();

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		var resultData = Assert.IsType<Result<PlanParticipantModel>>(result).Data;
		Assert.Equal(firstPlanParticipant.Prop, resultData.Prop);
		Assert.Equal(firstPlanParticipant.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.CreatedAt);

		_mock.PlanRepository.Verify(repo => repo.zzzAsync(It.IsAny<Plan>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.zzzAsync(It.IsAny<PlanParticipant>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.zzzAsync(It.IsAny<PlanParticipant>()), Times.Once);
	}

	[Fact]
	public async Task Test_RemoveUserFromPlanUseCase_UserCannotBeRemovedFromCancelledPlan_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RemoveUserFromPlanUseCase>();
		var firstPlanParticipant = _mock.PlanParticipants[0];
		var command = new RemoveUserFromPlanCommand();

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		var resultData = Assert.IsType<Result<PlanParticipantModel>>(result).Data;
		Assert.Equal(firstPlanParticipant.Prop, resultData.Prop);
		Assert.Equal(firstPlanParticipant.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.CreatedAt);

		_mock.PlanRepository.Verify(repo => repo.zzzAsync(It.IsAny<Plan>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.zzzAsync(It.IsAny<PlanParticipant>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.zzzAsync(It.IsAny<PlanParticipant>()), Times.Once);
	}*/
}
