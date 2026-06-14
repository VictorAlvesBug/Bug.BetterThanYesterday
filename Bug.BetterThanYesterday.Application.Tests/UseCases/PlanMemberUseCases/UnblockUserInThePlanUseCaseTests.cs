using Bug.BetterThanYesterday.Application.Mocks;
using Bug.BetterThanYesterday.Application.PlanMembers;
using Bug.BetterThanYesterday.Application.PlanMembers.UnblockUserInThePlan;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;
using Bug.BetterThanYesterday.Domain.PlanMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.PlanMemberUseCases;

public class UnblockUserInThePlanUseCaseTests : BasePlanMemberUseCaseTests
{
	[Fact]
	public async Task Test_UnblockUserInThePlanUseCase_MemberSuccessfullyUnblocked_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UnblockUserInThePlanUseCase>();
		var plan = _mock.Plans.First(plan => plan.Id == MockData.PublicRunningPlanId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active);
		var user = _mock.Users.First(user => user.Id == MockData.UserId3);
		var planMemberId = PlanMember.BuildId(plan.Id, user.Id);
		var planMember = _mock.PlanMembers.First(pp => pp.Id == planMemberId);
		var command = new UnblockUserInThePlanCommand(
			plan.Id,
			user.Id
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.MemberSuccessfullyUnblocked, result.GetMessage());

		var resultData = Assert.IsType<Result<PlanMemberDetailsModel>>(result).Data;
		
		Assert.Equal(plan.Id, resultData.Plan.Id);
		Assert.Equal(plan.OwnerId, resultData.Plan.OwnerId);
		Assert.Equal(plan.HabitId, resultData.Plan.HabitId);
		Assert.Equal(plan.Description, resultData.Plan.Description);
		Assert.Equal(plan.StartsAt.ToDateTime(TimeOnly.MinValue), resultData.Plan.StartsAt);
		Assert.Equal(plan.EndsAt.ToDateTime(TimeOnly.MinValue), resultData.Plan.EndsAt);
		Assert.Equal(plan.GetStatus(), PlanStatus.Get(resultData.Plan.Status));
		Assert.Equal(plan.Type, PlanType.Get(resultData.Plan.Type));
		Assert.Equal(plan.DaysOffPerWeek, resultData.Plan.DaysOffPerWeek);
		Assert.Equal(plan.PenaltyValue, resultData.Plan.PenaltyValue);
		Assert.Equal(plan.CreatedAt, resultData.Plan.CreatedAt);
		
		Assert.Equal(user.Id, resultData.User.Id);
		Assert.Equal(user.Name, resultData.User.Name);
		Assert.Equal(user.Email.Value, resultData.User.Email);
		Assert.Equal(user.CreatedAt, resultData.User.CreatedAt);

		Assert.Equal(planMember.Id, resultData.Id);
		Assert.Equal(planMember.JoinedAt.ToDateTime(TimeOnly.MinValue), resultData.JoinedAt);
		Assert.Equal(PlanMemberStatus.Active.Name, resultData.Status);

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanMember>()), Times.Once);
	}

	[Fact]
	public async Task Test_UnblockUserInThePlanUseCase_PlanNotFound_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UnblockUserInThePlanUseCase>();
		var command = new UnblockUserInThePlanCommand(
			Guid.NewGuid(),
			MockData.UserId3
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
	public async Task Test_UnblockUserInThePlanUseCase_UserNotFound_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UnblockUserInThePlanUseCase>();
		var command = new UnblockUserInThePlanCommand(
			MockData.PublicRunningPlanId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active,
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
	public async Task Test_UnblockUserInThePlanUseCase_UserIsNotInThePlan_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UnblockUserInThePlanUseCase>();
		var command = new UnblockUserInThePlanCommand(
			MockData.PublicRunningPlanId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active,
			MockData.UserId2
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
	public async Task Test_UnblockUserInThePlanUseCase_MemberIsAlreadyActiveInThePlan_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UnblockUserInThePlanUseCase>();
		var command = new UnblockUserInThePlanCommand(
			MockData.PublicRunningPlanId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active,
			MockData.UserId5
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.MemberIsAlreadyActiveInThePlan, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanMember>()), Times.Never);
	}

	[Fact]
	public async Task Test_BlockUserInThePlanUseCase_UserCannotBeUnblockedInNotStartedPlan_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UnblockUserInThePlanUseCase>();
		var command = new UnblockUserInThePlanCommand(
			MockData.PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active,
			MockData.UserId2
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.MemberCanOnlyBeUnblockedInRunningPlans, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanMember>()), Times.Never);
	}

	[Fact]
	public async Task Test_BlockUserInThePlanUseCase_UserCannotBeUnblockedInFinishedPlan_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UnblockUserInThePlanUseCase>();
		var command = new UnblockUserInThePlanCommand(
			MockData.PrivateFinishedPlanId4_WithUserId2ActiveAndUserId3Blocked,
			MockData.UserId3
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.MemberCanOnlyBeUnblockedInRunningPlans, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanMember>()), Times.Never);
	}

	[Fact]
	public async Task Test_BlockUserInThePlanUseCase_UserCannotBeUnblockedInCancelledPlan_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UnblockUserInThePlanUseCase>();
		var command = new UnblockUserInThePlanCommand(
			MockData.PublicCancelledPlanId6_WithUserId4BlockedAndUserId5Active,
			MockData.UserId4
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.MemberCanOnlyBeUnblockedInRunningPlans, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlanMember>()), Times.Never);
	}
}
