using Bug.BetterThanYesterday.Application.Plans.CancelPlan;
using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Moq;
using Xunit;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.PlanUseCases;

public class CancelPlanUseCaseTests : BasePlanUseCaseTests
{
	[Fact]
	public async Task Test_CancelPlanUseCase_Valid_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CancelPlanUseCase>();
		var firstPlan = _mock.Plans[0];
		var command = new CancelPlanCommand(firstPlan.Id);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.PlanSuccessfullyCancelled, result.GetMessage());

		var resultData = Assert.IsType<Result<PlanModel>>(result).Data;
		Assert.Equal(firstPlan.Id, resultData.PlanId);
		Assert.Equal(firstPlan.HabitId, resultData.HabitId);
		Assert.Equal(firstPlan.Description, resultData.Description);
		Assert.Equal(firstPlan.StartsAt.ToDateTime(TimeOnly.MinValue), resultData.StartsAt);
		Assert.Equal(firstPlan.EndsAt.ToDateTime(TimeOnly.MinValue), resultData.EndsAt);
		Assert.Equal(firstPlan.Status, PlanStatus.Cancelled);
		Assert.Equal(firstPlan.Type.Id, resultData.TypeId);
		Assert.Equal(firstPlan.Type.Name, resultData.TypeName);
		Assert.Equal(firstPlan.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.CreatedAt);

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Plan>()), Times.Once);
	}

	[Fact]
	public async Task Test_CancelPlanUseCase_NotFoundPlanId_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CancelPlanUseCase>();
		var command = new CancelPlanCommand(Guid.NewGuid());

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.PlanNotFound, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Plan>()), Times.Never);
	}
}
