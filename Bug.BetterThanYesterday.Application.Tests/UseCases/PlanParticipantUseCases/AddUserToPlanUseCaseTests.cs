using Bug.BetterThanYesterday.Application.PlanParticipants;
using Bug.BetterThanYesterday.Application.PlanParticipants.AddUserToPlan;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.PlanParticipantUseCases;

public class AddUserToPlanUseCaseTests : BasePlanParticipantUseCaseTests
{
	private readonly DateTime _today = DateTime.Today;

	[Fact]
	public async Task Test_AddUserToPlanUseCase_Valid_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<AddUserToPlanUseCase>();
		var plan = _mock.Plans.First(plan => plan.Id == PlanRepositoryMockFactory.PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Left);
		var user = _mock.Users.First(user => user.Id == UserRepositoryMockFactory.UserId4);
		var command = new AddUserToPlanCommand(
			plan.Id,
			user.Id
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());

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

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.AddAsync(It.IsAny<PlanParticipant>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanParticipant>()), Times.Never);
	}

	[Fact]
	public async Task Test_AddUserToPlanUseCase_NotFoundPlanId_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<AddUserToPlanUseCase>();
		var user = _mock.Users.First(user => user.Id == UserRepositoryMockFactory.UserId3);
		var command = new AddUserToPlanCommand(
			Guid.NewGuid(),
			user.Id
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.PlanParticipantRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.PlanParticipantRepository.Verify(repo => repo.AddAsync(It.IsAny<PlanParticipant>()), Times.Never);
		_mock.PlanParticipantRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanParticipant>()), Times.Never);
	}

	[Fact]
	public async Task Test_AddUserToPlanUseCase_NotFoundUserId_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<AddUserToPlanUseCase>();
		var plan = _mock.Plans.First(plan => plan.Id == PlanRepositoryMockFactory.PublicCancelledPlanId3);
		var command = new AddUserToPlanCommand(
			plan.Id,
			Guid.NewGuid()
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.PlanParticipantRepository.Verify(repo => repo.AddAsync(It.IsAny<PlanParticipant>()), Times.Never);
		_mock.PlanParticipantRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanParticipant>()), Times.Never);
	}

	[Fact]
	public async Task Test_AddUserToPlanUseCase_UserIsAlreadyInThePlanWithActiveStatus_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<AddUserToPlanUseCase>();
		var plan = _mock.Plans.First(plan => plan.Id == PlanRepositoryMockFactory.PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Left);
		var user = _mock.Users.First(user => user.Id == UserRepositoryMockFactory.UserId1);
		var command = new AddUserToPlanCommand(
			plan.Id,
			user.Id
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.AddAsync(It.IsAny<PlanParticipant>()), Times.Never);
		_mock.PlanParticipantRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanParticipant>()), Times.Never);
	}

	[Fact]
	public async Task Test_AddUserToPlanUseCase_UserIsAlreadyInThePlanWithBlockedStatus_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<AddUserToPlanUseCase>();
		var plan = _mock.Plans.First(plan => plan.Id == PlanRepositoryMockFactory.PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Left);
		var user = _mock.Users.First(user => user.Id == UserRepositoryMockFactory.UserId2);
		var command = new AddUserToPlanCommand(
			plan.Id,
			user.Id
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.AddAsync(It.IsAny<PlanParticipant>()), Times.Never);
		_mock.PlanParticipantRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanParticipant>()), Times.Never);
	}

	[Fact]
	public async Task Test_AddUserToPlanUseCase_UserHasAlreadyBeenOnThisPlanBefore_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<AddUserToPlanUseCase>();
		var plan = _mock.Plans.First(plan => plan.Id == PlanRepositoryMockFactory.PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Left);
		var user = _mock.Users.First(user => user.Id == UserRepositoryMockFactory.UserId3);
		var command = new AddUserToPlanCommand(
			plan.Id,
			user.Id
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());

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

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.AddAsync(It.IsAny<PlanParticipant>()), Times.Never);
		_mock.PlanParticipantRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanParticipant>()), Times.Once);
	}

	[Fact]
	public async Task Test_AddUserToPlanUseCase_UserCannotBeAddedToRunningPlan_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<AddUserToPlanUseCase>();
		var plan = _mock.Plans.First(plan => plan.Id == PlanRepositoryMockFactory.PublicRunningPlanId1_WithUserId1Active);
		var user = _mock.Users.First(user => user.Id == UserRepositoryMockFactory.UserId3);
		var command = new AddUserToPlanCommand(
			plan.Id,
			user.Id
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.PlanParticipantRepository.Verify(repo => repo.AddAsync(It.IsAny<PlanParticipant>()), Times.Never);
		_mock.PlanParticipantRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanParticipant>()), Times.Never);
	}

	[Fact]
	public async Task Test_AddUserToPlanUseCase_UserCannotBeAddedToFinishedPlan_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<AddUserToPlanUseCase>();
		var plan = _mock.Plans.First(plan => plan.Id == PlanRepositoryMockFactory.PrivateFinishedPlanId4_WithUserId2LeftAndUserId3Blocked);
		var user = _mock.Users.First(user => user.Id == UserRepositoryMockFactory.UserId1);
		var command = new AddUserToPlanCommand(
			plan.Id,
			user.Id
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.PlanParticipantRepository.Verify(repo => repo.AddAsync(It.IsAny<PlanParticipant>()), Times.Never);
		_mock.PlanParticipantRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanParticipant>()), Times.Never);
	}

	[Fact]
	public async Task Test_AddUserToPlanUseCase_UserCannotBeAddedToCancelledPlan_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<AddUserToPlanUseCase>();
		var plan = _mock.Plans.First(plan => plan.Id == PlanRepositoryMockFactory.PublicCancelledPlanId3);
		var user = _mock.Users.First(user => user.Id == UserRepositoryMockFactory.UserId1);
		var command = new AddUserToPlanCommand(
			plan.Id,
			user.Id
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.PlanParticipantRepository.Verify(repo => repo.AddAsync(It.IsAny<PlanParticipant>()), Times.Never);
		_mock.PlanParticipantRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanParticipant>()), Times.Never);
	}
}
