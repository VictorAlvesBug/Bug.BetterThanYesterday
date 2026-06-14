using Bug.BetterThanYesterday.Application.Mocks;
using Bug.BetterThanYesterday.Application.PlanMembers;
using Bug.BetterThanYesterday.Application.PlanMembers.GetPlanWithMembersByPlanId;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.PlanMemberUseCases;

public class GetPlanWithMembersByPlanIdUseCaseTests : BasePlanMemberUseCaseTests
{
	[Fact]
	public async Task Test_GetPlanWithMembersByPlanIdUseCase_PlanSuccessfullyFound_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetPlanWithMembersByPlanIdUseCase>();
		var planId = _mock.PlanMembers.First().PlanId;
		var plan = _mock.Plans.First(plan => plan.Id == planId);
		var owner = _mock.Users.First(user => user.Id == plan.OwnerId);
		var users =
			(
				from
					planMember in _mock.PlanMembers
				where
					planMember.PlanId == planId
				join user in _mock.Users 
					on planMember.UserId equals user.Id
				select
					user
			).Distinct()
			.ToList();
		var command = new GetPlanWithMembersByPlanIdCommand(planId);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.PlanSuccessfullyFound, result.GetMessage());

		var resultData = Assert.IsType<Result<PlanWithMembersModel>>(result).Data;

		Assert.Equal(plan.Id, resultData.Plan.Id);
		Assert.Equal(plan.OwnerId, resultData.Plan.OwnerId);
		Assert.Equal(owner.Name, resultData.Plan.OwnerName);
		Assert.Equal(plan.HabitId, resultData.Plan.HabitId);
		Assert.Equal(plan.Description, resultData.Plan.Description);
		Assert.Equal(plan.StartsAt.ToDateTime(TimeOnly.MinValue), resultData.Plan.StartsAt);
		Assert.Equal(plan.EndsAt.ToDateTime(TimeOnly.MinValue), resultData.Plan.EndsAt);
		Assert.Equal(plan.GetStatus(), PlanStatus.Get(resultData.Plan.Status));
		Assert.Equal(plan.Type, PlanType.Get(resultData.Plan.Type));
		Assert.Equal(plan.DaysOffPerWeek, resultData.Plan.DaysOffPerWeek);
		Assert.Equal(plan.PenaltyValue, resultData.Plan.PenaltyValue);
		Assert.Equal(plan.CreatedAt, resultData.Plan.CreatedAt);
		
		Assert.Equal(users.Count, resultData.Users.Count);

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.ListByPlanIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()), Times.Once);
	}

	[Fact]
	public async Task Test_GetPlanWithMembersByPlanIdUseCase_PlanHasNoMembers_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetPlanWithMembersByPlanIdUseCase>();
		var plan = _mock.Plans.First(plan => plan.Id == MockData.PublicCancelledPlanId3);
		var owner = _mock.Users.First(user => user.Id == plan.OwnerId);
		var command = new GetPlanWithMembersByPlanIdCommand(plan.Id);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.PlanHasNoMembers, result.GetMessage());

		var resultData = Assert.IsType<Result<PlanWithMembersModel>>(result).Data;

		Assert.Equal(plan.Id, resultData.Plan.Id);
		Assert.Equal(plan.OwnerId, resultData.Plan.OwnerId);
		Assert.Equal(owner.Name, resultData.Plan.OwnerName);
		Assert.Equal(plan.HabitId, resultData.Plan.HabitId);
		Assert.Equal(plan.Description, resultData.Plan.Description);
		Assert.Equal(plan.StartsAt.ToDateTime(TimeOnly.MinValue), resultData.Plan.StartsAt);
		Assert.Equal(plan.EndsAt.ToDateTime(TimeOnly.MinValue), resultData.Plan.EndsAt);
		Assert.Equal(plan.GetStatus(), PlanStatus.Get(resultData.Plan.Status));
		Assert.Equal(plan.Type, PlanType.Get(resultData.Plan.Type));
		Assert.Equal(plan.DaysOffPerWeek, resultData.Plan.DaysOffPerWeek);
		Assert.Equal(plan.PenaltyValue, resultData.Plan.PenaltyValue);
		Assert.Equal(plan.CreatedAt, resultData.Plan.CreatedAt);
		
		Assert.Empty(resultData.Users);

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.ListByPlanIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()), Times.Never);
	}

	[Fact]
	public async Task Test_GetPlanWithMembersByPlanIdUseCase_PlanNotFound_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetPlanWithMembersByPlanIdUseCase>();
		var command = new GetPlanWithMembersByPlanIdCommand(Guid.NewGuid());

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.PlanNotFound, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.ListByPlanIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.UserRepository.Verify(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()), Times.Never);
	}
}
