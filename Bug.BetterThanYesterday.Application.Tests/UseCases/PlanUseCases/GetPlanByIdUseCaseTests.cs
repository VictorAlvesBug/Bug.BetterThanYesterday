using Bug.BetterThanYesterday.Application.Plans.GetPlanById;
using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Moq;
using Xunit;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.PlanUseCases;

public class GetPlanByIdUseCaseTests : BasePlanUseCaseTests
{
	[Fact]
	public async Task Test_GetPlanByIdUseCase_PlanSuccessfullyFound_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetPlanByIdUseCase>();
		var firstPlan = _mock.Plans[0];
		var owner = _mock.Users.First(user => user.Id == firstPlan.OwnerId);
		var command = new GetPlanByIdCommand(firstPlan.Id);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.PlanSuccessfullyFound, result.GetMessage());

		var resultData = Assert.IsType<Result<PlanModel>>(result).Data;

		Assert.Equal(firstPlan.Id, resultData.PlanId);
		Assert.Equal(firstPlan.OwnerId, resultData.OwnerId);
		Assert.Equal(owner.Name, resultData.OwnerName);
		Assert.Equal(firstPlan.HabitId, resultData.HabitId);
		Assert.Equal(firstPlan.Description, resultData.Description);
		Assert.Equal(firstPlan.StartsAt.ToDateTime(TimeOnly.MinValue), resultData.StartsAt);
		Assert.Equal(firstPlan.EndsAt.ToDateTime(TimeOnly.MinValue), resultData.EndsAt);
		Assert.Equal(firstPlan.GetStatus(), PlanStatus.Get(resultData.Status));
		Assert.Equal(firstPlan.Type, PlanType.Get(resultData.Type));
		Assert.Equal(firstPlan.DaysOffPerWeek, resultData.DaysOffPerWeek);
		Assert.Equal(firstPlan.PenaltyValue, resultData.PenaltyValue);
		Assert.Equal(firstPlan.CreatedAt, resultData.CreatedAt);

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
	}

	[Fact]
	public async Task Test_GetPlanByIdUseCase_PlanNotFound_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetPlanByIdUseCase>();
		var command = new GetPlanByIdCommand(Guid.NewGuid());

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.PlanNotFound, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
	}
}
