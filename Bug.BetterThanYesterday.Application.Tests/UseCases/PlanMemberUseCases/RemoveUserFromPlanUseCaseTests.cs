using Bug.BetterThanYesterday.Application.PlanMembers;
using Bug.BetterThanYesterday.Application.PlanMembers.RemoveUserFromPlan;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;
using Bug.BetterThanYesterday.Domain.PlanMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.PlanMemberUseCases;

public class RemoveUserFromPlanUseCaseTests : BasePlanMemberUseCaseTests
{
	[Fact]
	public async Task Test_RemoveUserFromPlanUseCase_MemberSuccessfullyRemovedFromThePlan_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RemoveUserFromPlanUseCase>();
		var plan = _mock.Plans.First(plan => plan.Id == PlanRepositoryMockFactory.PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Left);
		var user = _mock.Users.First(user => user.Id == UserRepositoryMockFactory.UserId1);
		var planMemberId = PlanMember.BuildId(plan.Id, user.Id);
		var planMember = _mock.PlanMembers.First(pp => pp.Id == planMemberId);
		var command = new RemoveUserFromPlanCommand(
			plan.Id,
			user.Id
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.MemberSuccessfullyRemovedFromThePlan, result.GetMessage());

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
		Assert.Equal(plan.CreatedAt, resultData.Plan.CreatedAt);
		
		Assert.Equal(user.Id, resultData.Member.UserId);
		Assert.Equal(user.Name, resultData.Member.Name);
		Assert.Equal(user.Email.Value, resultData.Member.Email);
		Assert.Equal(user.CreatedAt, resultData.Member.CreatedAt);

		Assert.Equal(planMember.Id, resultData.PlanMemberId);
		Assert.Equal(planMember.JoinedAt.ToDateTime(TimeOnly.MinValue), resultData.JoinedAt);
		Assert.Equal(DateTime.Today, resultData.LeftAt);
		Assert.Equal(PlanMemberStatus.Left.Id, resultData.StatusId);
		Assert.Equal(PlanMemberStatus.Left.Name, resultData.StatusName);

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanMember>()), Times.Once);
	}

	[Fact]
	public async Task Test_RemoveUserFromPlanUseCase_PlanNotFound_ShouldReturnRejected()
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
		_mock.PlanMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.PlanMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanMember>()), Times.Never);
	}

	[Fact]
	public async Task Test_RemoveUserFromPlanUseCase_UserNotFound_ShouldReturnRejected()
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
		_mock.PlanMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.PlanMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanMember>()), Times.Never);
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
		_mock.PlanMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanMember>()), Times.Never);
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
		_mock.PlanMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanMember>()), Times.Never);
	}

	[Fact]
	public async Task Test_RemoveUserFromPlanUseCase_MemberCannotBeRemovedFromThePlanAsHeIsBlocked_ShouldReturnRejected()
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
		Assert.Equal(Messages.MemberCannotBeRemovedFromThePlanAsHeIsBlocked, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanMember>()), Times.Never);
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
		Assert.Equal(Messages.MemberCanOnlyBeRemovedFromNotStartedOrRunningPlans, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanMember>()), Times.Never);
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
		Assert.Equal(Messages.MemberCanOnlyBeRemovedFromNotStartedOrRunningPlans, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanMember>()), Times.Never);
	}
}
