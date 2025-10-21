using Bug.BetterThanYesterday.Application.PlanParticipants;
using Bug.BetterThanYesterday.Application.PlanParticipants.BlockUserInThePlan;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;
using Bug.BetterThanYesterday.Domain.PlanParticipants.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.PlanParticipantUseCases;

public class BlockUserInThePlanUseCaseTests : BasePlanParticipantUseCaseTests
{
	[Fact]
	public async Task Test_BlockUserInThePlanUseCase_Valid_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<BlockUserInThePlanUseCase>();
		var plan = _mock.Plans.First(plan => plan.Id == PlanRepositoryMockFactory.PublicRunningPlanId7_WithUserId3BlockedAndUserId4LeftAndUserId5Active);
		var user = _mock.Users.First(user => user.Id == UserRepositoryMockFactory.UserId5);
		var planParticipantId = PlanParticipant.BuildId(plan.Id, user.Id);
		var planParticipant = _mock.PlanParticipants.First(pp => pp.Id == planParticipantId);
		var command = new BlockUserInThePlanCommand(
			plan.Id,
			user.Id
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.ParticipantSuccessfullyBlocked, result.GetMessage());

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
		Assert.Null(resultData.LeftAt);
		Assert.Equal(PlanParticipantStatus.Blocked.Id, resultData.StatusId);
		Assert.Equal(PlanParticipantStatus.Blocked.Name, resultData.StatusName);

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanParticipant>()), Times.Once);
	}

	[Fact]
	public async Task Test_BlockUserInThePlanUseCase_NotFoundPlanId_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<BlockUserInThePlanUseCase>();
		var command = new BlockUserInThePlanCommand(
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
	public async Task Test_BlockUserInThePlanUseCase_NotFoundUserId_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<BlockUserInThePlanUseCase>();
		var command = new BlockUserInThePlanCommand(
			PlanRepositoryMockFactory.PublicRunningPlanId7_WithUserId3BlockedAndUserId4LeftAndUserId5Active,
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
	public async Task Test_BlockUserInThePlanUseCase_UserIsNotInThePlan_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<BlockUserInThePlanUseCase>();
		var command = new BlockUserInThePlanCommand(
			PlanRepositoryMockFactory.PublicRunningPlanId7_WithUserId3BlockedAndUserId4LeftAndUserId5Active,
			UserRepositoryMockFactory.UserId2
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
	public async Task Test_BlockUserInThePlanUseCase_UserIsNotInThePlanAnymore_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<BlockUserInThePlanUseCase>();
		var command = new BlockUserInThePlanCommand(
			PlanRepositoryMockFactory.PublicRunningPlanId7_WithUserId3BlockedAndUserId4LeftAndUserId5Active,
			UserRepositoryMockFactory.UserId4
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
	public async Task Test_BlockUserInThePlanUseCase_UserIsAlreadyBlocked_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<BlockUserInThePlanUseCase>();
		var command = new BlockUserInThePlanCommand(
			PlanRepositoryMockFactory.PublicRunningPlanId7_WithUserId3BlockedAndUserId4LeftAndUserId5Active,
			UserRepositoryMockFactory.UserId3
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.ParticipantAlreadyBlockedInThisPlan, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanParticipant>()), Times.Never);
	}

	[Fact]
	public async Task Test_BlockUserInThePlanUseCase_UserCannotBeBlockedInNotStartedPlan_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<BlockUserInThePlanUseCase>();
		var command = new BlockUserInThePlanCommand(
			PlanRepositoryMockFactory.PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Left,
			UserRepositoryMockFactory.UserId1
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.ParticipantCanOnlyBeBlockedInRunningPlans, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanParticipant>()), Times.Never);
	}

	[Fact]
	public async Task Test_BlockUserInThePlanUseCase_UserCannotBeBlockedInFinishedPlan_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<BlockUserInThePlanUseCase>();
		var command = new BlockUserInThePlanCommand(
			PlanRepositoryMockFactory.PrivateFinishedPlanId5_WithUserId5Active,
			UserRepositoryMockFactory.UserId5
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.ParticipantCanOnlyBeBlockedInRunningPlans, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanParticipant>()), Times.Never);
	}

	[Fact]
	public async Task Test_BlockUserInThePlanUseCase_UserCannotBeBlockedInCancelledPlan_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<BlockUserInThePlanUseCase>();
		var command = new BlockUserInThePlanCommand(
			PlanRepositoryMockFactory.PublicCancelledPlanId6_WithUserId4BlockedAndUserId5Active,
			UserRepositoryMockFactory.UserId5
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.ParticipantCanOnlyBeBlockedInRunningPlans, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanParticipant>()), Times.Never);
	}
}
