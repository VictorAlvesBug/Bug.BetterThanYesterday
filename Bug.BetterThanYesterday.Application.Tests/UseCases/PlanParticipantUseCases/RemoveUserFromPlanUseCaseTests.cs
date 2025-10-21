using Bug.BetterThanYesterday.Application.PlanParticipants;
using Bug.BetterThanYesterday.Application.PlanParticipants.RemoveUserFromPlan;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;
using Bug.BetterThanYesterday.Domain.PlanParticipants.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.PlanParticipantUseCases;

public class RemoveUserFromPlanUseCaseTests : BasePlanParticipantUseCaseTests
{
	[Fact]
	public async Task Test_RemoveUserFromPlanUseCase_Valid_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RemoveUserFromPlanUseCase>();
		var plan = _mock.Plans.First(plan => plan.Id == PlanRepositoryMockFactory.PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Left);
		var user = _mock.Users.First(user => user.Id == UserRepositoryMockFactory.UserId1);
		var planParticipantId = PlanParticipant.BuildId(plan.Id, user.Id);
		var planParticipant = _mock.PlanParticipants.First(pp => pp.Id == planParticipantId);
		var command = new RemoveUserFromPlanCommand(
			plan.Id,
			user.Id
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.ParticipantSuccessfullyRemovedFromThePlan, result.GetMessage());

		var resultData = Assert.IsType<Result<PlanParticipantDetailsModel>>(result).Data;
		
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
		
		Assert.Equal(user.Id, resultData.Participant.UserId);
		Assert.Equal(user.Name, resultData.Participant.Name);
		Assert.Equal(user.Email.Value, resultData.Participant.Email);
		Assert.Equal(user.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.Participant.CreatedAt);

		Assert.Equal(planParticipant.Id, resultData.PlanParticipantId);
		Assert.Equal(planParticipant.JoinedAt.ToDateTime(TimeOnly.MinValue), resultData.JoinedAt);
		Assert.Equal(DateTime.Today, resultData.LeftAt);
		Assert.Equal(PlanParticipantStatus.Left.Id, resultData.StatusId);
		Assert.Equal(PlanParticipantStatus.Left.Name, resultData.StatusName);

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanParticipant>()), Times.Once);
	}

	[Fact]
	public async Task Test_RemoveUserFromPlanUseCase_NotFoundPlanId_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RemoveUserFromPlanUseCase>();
		var command = new RemoveUserFromPlanCommand(
			Guid.NewGuid(),
			UserRepositoryMockFactory.UserId1
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.PlanNotFound, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.PlanParticipantRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.PlanParticipantRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanParticipant>()), Times.Never);
	}

	[Fact]
	public async Task Test_RemoveUserFromPlanUseCase_NotFoundUserId_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RemoveUserFromPlanUseCase>();
		var command = new RemoveUserFromPlanCommand(
			PlanRepositoryMockFactory.PublicRunningPlanId1_WithUserId1Active,
			Guid.NewGuid()
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.UserNotFound, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.PlanParticipantRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanParticipant>()), Times.Never);
	}

	[Fact]
	public async Task Test_RemoveUserFromPlanUseCase_UserIsNotInThePlan_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RemoveUserFromPlanUseCase>();
		var command = new RemoveUserFromPlanCommand(
			PlanRepositoryMockFactory.PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Left,
			UserRepositoryMockFactory.UserId4
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.UserIsNotInThePlan, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanParticipant>()), Times.Never);
	}

	[Fact]
	public async Task Test_RemoveUserFromPlanUseCase_UserIsNotInThePlanAnymore_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RemoveUserFromPlanUseCase>();
		var command = new RemoveUserFromPlanCommand(
			PlanRepositoryMockFactory.PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Left,
			UserRepositoryMockFactory.UserId3
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.UserIsNotInThePlanAnymore, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanParticipant>()), Times.Never);
	}

	[Fact]
	public async Task Test_RemoveUserFromPlanUseCase_UserIsBlockedInThePlan_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RemoveUserFromPlanUseCase>();
		var command = new RemoveUserFromPlanCommand(
			PlanRepositoryMockFactory.PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Left,
			UserRepositoryMockFactory.UserId2
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.ParticipantCannotBeRemovedFromThePlanAsHeIsBlocked, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanParticipant>()), Times.Never);
	}

	[Fact]
	public async Task Test_RemoveUserFromPlanUseCase_UserCannotBeRemovedFromFinishedPlan_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RemoveUserFromPlanUseCase>();
		var command = new RemoveUserFromPlanCommand(
			PlanRepositoryMockFactory.PrivateFinishedPlanId5_WithUserId5Active,
			UserRepositoryMockFactory.UserId5
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.ParticipantCanOnlyBeRemovedFromNotStartedOrRunningPlans, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanParticipant>()), Times.Never);
	}

	[Fact]
	public async Task Test_RemoveUserFromPlanUseCase_UserCannotBeRemovedFromCancelledPlan_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RemoveUserFromPlanUseCase>();
		var command = new RemoveUserFromPlanCommand(
			PlanRepositoryMockFactory.PublicCancelledPlanId6_WithUserId4BlockedAndUserId5Active,
			UserRepositoryMockFactory.UserId5
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.ParticipantCanOnlyBeRemovedFromNotStartedOrRunningPlans, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanParticipant>()), Times.Never);
	}
}
