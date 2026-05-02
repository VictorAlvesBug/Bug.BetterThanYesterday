using Bug.BetterThanYesterday.Application.PlanMembers;
using Bug.BetterThanYesterday.Application.PlanMembers.GetPlanWithMembersByPlanId;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Tests.Commons;
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
		var members =
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
		
		Assert.Equal(members.Count, resultData.Members.Count);

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanMemberRepository.Verify(repo => repo.ListByPlanIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()), Times.Once);
	}

	[Fact]
	public async Task Test_GetPlanWithMembersByPlanIdUseCase_PlanHasNoMembers_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetPlanWithMembersByPlanIdUseCase>();
		var plan = _mock.Plans.First(plan => plan.Id == PlanRepositoryMockFactory.PublicCancelledPlanId3);
		var command = new GetPlanWithMembersByPlanIdCommand(plan.Id);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.PlanHasNoMembers, result.GetMessage());

		var resultData = Assert.IsType<Result<PlanWithMembersModel>>(result).Data;
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
		
		Assert.Empty(resultData.Members);

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
