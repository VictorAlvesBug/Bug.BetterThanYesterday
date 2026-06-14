using Bug.BetterThanYesterday.Application.Mocks;
using Bug.BetterThanYesterday.Application.PlanMembers;
using Bug.BetterThanYesterday.Application.PlanMembers.AddUserToPlan;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
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
		var plan = _mock.Plans.First(plan => plan.Id == MockData.PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active);
		var user = _mock.Users.First(user => user.Id == MockData.UserId4);
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
		var user = _mock.Users.First(user => user.Id == MockData.UserId3);
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
		var plan = _mock.Plans.First(plan => plan.Id == MockData.PublicCancelledPlanId3);
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
		var plan = _mock.Plans.First(plan => plan.Id == MockData.PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active);
		var user = _mock.Users.First(user => user.Id == MockData.UserId1);
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
		var plan = _mock.Plans.First(plan => plan.Id == MockData.PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active);
		var user = _mock.Users.First(user => user.Id == MockData.UserId2);
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
		var plan = _mock.Plans.First(plan => plan.Id == MockData.PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active);
		var user = _mock.Users.First(user => user.Id == MockData.UserId3);
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
		var plan = _mock.Plans.First(plan => plan.Id == MockData.PublicRunningPlanId1_WithUserId1Active);
		var user = _mock.Users.First(user => user.Id == MockData.UserId3);
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
		var plan = _mock.Plans.First(plan => plan.Id == MockData.PrivateFinishedPlanId4_WithUserId2ActiveAndUserId3Blocked);
		var user = _mock.Users.First(user => user.Id == MockData.UserId1);
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
		var plan = _mock.Plans.First(plan => plan.Id == MockData.PublicCancelledPlanId3);
		var user = _mock.Users.First(user => user.Id == MockData.UserId1);
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
