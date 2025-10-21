using Bug.BetterThanYesterday.Application.Plans.GetPlanById;
using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Moq;
using Xunit;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.PlanUseCases;

public class GetPlanByIdUseCaseTests : BasePlanUseCaseTests
{
	[Fact]
	public async Task Test_GetPlanByIdUseCase_PlanSuccessfullyFound_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetPlanByIdUseCase>();
		var firstPlan = _mock.Plans[0];
		var command = new GetPlanByIdCommand(firstPlan.Id);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.PlanSuccessfullyFound, result.GetMessage());

		var resultData = Assert.IsType<Result<PlanModel>>(result).Data;
		Assert.Equal(firstPlan.Id, resultData.PlanId);
		Assert.Equal(firstPlan.HabitId, resultData.HabitId);
		Assert.Equal(firstPlan.Description, resultData.Description);
		Assert.Equal(firstPlan.StartsAt.ToDateTime(TimeOnly.MinValue), resultData.StartsAt);
		Assert.Equal(firstPlan.EndsAt.ToDateTime(TimeOnly.MinValue), resultData.EndsAt);
		Assert.Equal(firstPlan.Status.Id, resultData.StatusId);
		Assert.Equal(firstPlan.Status.Name, resultData.StatusName);
		Assert.Equal(firstPlan.Type.Id, resultData.TypeId);
		Assert.Equal(firstPlan.Type.Name, resultData.TypeName);
		Assert.Equal(firstPlan.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.CreatedAt);

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
