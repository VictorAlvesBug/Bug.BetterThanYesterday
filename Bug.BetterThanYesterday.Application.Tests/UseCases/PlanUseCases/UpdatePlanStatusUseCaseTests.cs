using Bug.BetterThanYesterday.Application.Plans.UpdatePlanStatus;
using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Moq;
using Xunit;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.PlanUseCases;

public class UpdatePlanStatusUseCaseTests : BasePlanUseCaseTests
{
	[Fact]
	public async Task Test_UpdatePlanStatusUseCase_PlanStatusSuccessfullyUpdated_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UpdatePlanStatusUseCase>();
		var firstPlan = _mock.Plans[0];
		var newStatus = PlanStatus.Finished;
		var command = new UpdatePlanStatusCommand(firstPlan.Id, newStatus.Id);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.PlanStatusSuccessfullyUpdated, result.GetMessage());

		var resultData = Assert.IsType<Result<PlanModel>>(result).Data;
		Assert.Equal(firstPlan.Id, resultData.PlanId);
		Assert.Equal(firstPlan.HabitId, resultData.HabitId);
		Assert.Equal(firstPlan.Description, resultData.Description);
		Assert.Equal(firstPlan.StartsAt.ToDateTime(TimeOnly.MinValue), resultData.StartsAt);
		Assert.Equal(firstPlan.EndsAt.ToDateTime(TimeOnly.MinValue), resultData.EndsAt);
		Assert.Equal(newStatus.Id, resultData.StatusId);
		Assert.Equal(newStatus.Name, resultData.StatusName);
		Assert.Equal(firstPlan.Type.Id, resultData.TypeId);
		Assert.Equal(firstPlan.Type.Name, resultData.TypeName);

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Plan>()), Times.Once);
	}

	[Fact]
	public async Task Test_UpdatePlanStatusUseCase_PlanNotFound_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UpdatePlanStatusUseCase>();
		var command = new UpdatePlanStatusCommand(Guid.NewGuid(), PlanStatus.Running.Id);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.PlanNotFound, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Plan>()), Times.Never);
	}

	[Fact]
	public async Task Test_UpdatePlanStatusUseCase_EnterPlanStatus_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UpdatePlanStatusUseCase>();
		var firstPlan = _mock.Plans[0];
		var command = new UpdatePlanStatusCommand(firstPlan.Id, 0);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Contains(Messages.EnterPlanStatus, result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.PlanRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Plan>()), Times.Never);
	}

	[Fact]
	public async Task Test_UpdatePlanStatusUseCase_StatusIdNotAllowed_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UpdatePlanStatusUseCase>();
		var plan = _mock.Plans.First(plan => plan.Id == PlanRepositoryMockFactory.PublicCancelledPlanId3);
		var command = new UpdatePlanStatusCommand(plan.Id, PlanStatus.Finished.Id);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Contains("Não é possível alterar o status", result.GetMessage());

		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Plan>()), Times.Never);
	}
}
