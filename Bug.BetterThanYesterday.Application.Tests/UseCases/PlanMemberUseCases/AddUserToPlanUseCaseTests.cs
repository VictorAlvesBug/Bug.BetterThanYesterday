using Bug.BetterThanYesterday.Application.PlanMembers;
using Bug.BetterThanYesterday.Application.PlanMembers.AddUserToPlan;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;
using Bug.BetterThanYesterday.Domain.Strings;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.PlanMemberUseCases;

public class AddUserToPlanUseCaseTests : BasePlanMemberUseCaseTests
{
	private readonly DateTime _today = DateTime.Today;

	[Fact]
	public async Task Test_AddUserToPlanUseCase_MemberSuccessfullyAddedToThePlan_ShouldReturnSuccess()
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
		Assert.Equal(Messages.MemberSuccessfullyAddedToThePlan, result.GetMessage());

		var resultData = Assert.IsType<Result<PlanMemberDetailsModel>>(result).Data;
		
		Assert.Equal(plan.Id, resultData.Plan.PlanId);
		Assert.Equal(plan.HabitId, resultData.Plan.Habit.HabitId);
		Assert.Equal(plan.Description, resultData.Plan.Description);
		Assert.Equal(plan.StartsAt.ToDateTime(TimeOnly.MinValue), resultData.Plan.StartsAt);
		Assert.Equal(plan.EndsAt.ToDateTime(TimeOnly.MinValue), resultData.Plan.EndsAt);
		Assert.Equal(plan.Status.Id, resultData.Plan.StatusId);
		Assert.Equal(plan.Status.Name, resultData.Plan.StatusName);
		Assert.Equal(plan.Type.Id, resultData.Plan.TypeId);
		Assert.Equal(plan.Type.Name, resultData.Plan.TypeName);
		Assert.Equal(plan.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.Plan.CreatedAt);
		
		Assert.Equal(user.Id, resultData.Member.UserId);
		Assert.Equal(user.Name, resultData.Member.Name);
		Assert.Equal(user.Email.Value, resultData.Member.Email);
		Assert.Equal(user.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.Member.CreatedAt);

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.AddAsync(It.IsAny<PlanMember>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanMember>()), Times.Never);
	}

	[Fact]
	public async Task Test_AddUserToPlanUseCase_PlanNotFound_ShouldReturnRejected()
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
		Assert.Equal(Messages.PlanNotFound, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.PlanMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.PlanMemberRepository.Verify(repo => repo.AddAsync(It.IsAny<PlanMember>()), Times.Never);
		_mock.PlanMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanMember>()), Times.Never);
	}

	[Fact]
	public async Task Test_AddUserToPlanUseCase_UserNotFound_ShouldReturnRejected()
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
		Assert.Equal(Messages.UserNotFound, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.PlanMemberRepository.Verify(repo => repo.AddAsync(It.IsAny<PlanMember>()), Times.Never);
		_mock.PlanMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanMember>()), Times.Never);
	}

	[Fact]
	public async Task Test_AddUserToPlanUseCase_ActiveMemberAlreadyAddedToThePlan_ShouldReturnRejected()
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
		Assert.Equal(Messages.MemberAlreadyAddedToThePlan, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.AddAsync(It.IsAny<PlanMember>()), Times.Never);
		_mock.PlanMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanMember>()), Times.Never);
	}

	[Fact]
	public async Task Test_AddUserToPlanUseCase_BlockedMemberAlreadyAddedToThePlan_ShouldReturnRejected()
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
		Assert.Equal(Messages.MemberAlreadyAddedToThePlan, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.AddAsync(It.IsAny<PlanMember>()), Times.Never);
		_mock.PlanMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanMember>()), Times.Never);
	}

	[Fact]
	public async Task Test_AddUserToPlanUseCase_MemberSuccessfullyReaddedToThePlan_ShouldReturnSuccess()
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
		Assert.Equal(Messages.MemberSuccessfullyReaddedToThePlan, result.GetMessage());

		var resultData = Assert.IsType<Result<PlanMemberDetailsModel>>(result).Data;
		
		Assert.Equal(plan.Id, resultData.Plan.PlanId);
		Assert.Equal(plan.HabitId, resultData.Plan.Habit.HabitId);
		Assert.Equal(plan.Description, resultData.Plan.Description);
		Assert.Equal(plan.StartsAt.ToDateTime(TimeOnly.MinValue), resultData.Plan.StartsAt);
		Assert.Equal(plan.EndsAt.ToDateTime(TimeOnly.MinValue), resultData.Plan.EndsAt);
		Assert.Equal(plan.Status.Id, resultData.Plan.StatusId);
		Assert.Equal(plan.Status.Name, resultData.Plan.StatusName);
		Assert.Equal(plan.Type.Id, resultData.Plan.TypeId);
		Assert.Equal(plan.Type.Name, resultData.Plan.TypeName);
		Assert.Equal(plan.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.Plan.CreatedAt);
		
		Assert.Equal(user.Id, resultData.Member.UserId);
		Assert.Equal(user.Name, resultData.Member.Name);
		Assert.Equal(user.Email.Value, resultData.Member.Email);
		Assert.Equal(user.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.Member.CreatedAt);

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.AddAsync(It.IsAny<PlanMember>()), Times.Never);
		_mock.PlanMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanMember>()), Times.Once);
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
		Assert.Equal(Messages.OnlyNotStartedPlansCanReceiveNewMembers, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.PlanMemberRepository.Verify(repo => repo.AddAsync(It.IsAny<PlanMember>()), Times.Never);
		_mock.PlanMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanMember>()), Times.Never);
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
		Assert.Equal(Messages.OnlyNotStartedPlansCanReceiveNewMembers, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.PlanMemberRepository.Verify(repo => repo.AddAsync(It.IsAny<PlanMember>()), Times.Never);
		_mock.PlanMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanMember>()), Times.Never);
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
		Assert.Equal(Messages.OnlyNotStartedPlansCanReceiveNewMembers, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.PlanMemberRepository.Verify(repo => repo.AddAsync(It.IsAny<PlanMember>()), Times.Never);
		_mock.PlanMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanMember>()), Times.Never);
	}
}
