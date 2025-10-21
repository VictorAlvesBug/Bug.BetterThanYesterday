using Bug.BetterThanYesterday.Application.Plans.ListPlansByHabitId;
using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Moq;
using Xunit;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.PlanUseCases;

public class ListPlansByHabitIdUseCaseTests : BasePlanUseCaseTests
{
	[Fact]
	public async Task Test_ListPlansByHabitIdUseCase_PlansSuccessfullyFound_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<ListPlansByHabitIdUseCase>();
		var habit = _mock.Habits.First(h => h.Id == HabitRepositoryMockFactory.HabitId3);
		var command = new ListPlansByHabitIdCommand(habit.Id);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.PlansSuccessfullyFound, result.GetMessage());

		var resultData = Assert.IsType<Result<IEnumerable<PlanModel>>>(result).Data;
		Assert.Equal(_mock.Plans.Count(plan => plan.HabitId == habit.Id), resultData.Count());

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanRepository.Verify(repo => repo.ListByHabitIdAsync(It.IsAny<Guid>()), Times.Once);
	}

	[Fact]
	public async Task Test_ListPlansByHabitIdUseCase_HabitNotFound_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<ListPlansByHabitIdUseCase>();
		var command = new ListPlansByHabitIdCommand(Guid.NewGuid());

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.HabitNotFound, result.GetMessage());

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanRepository.Verify(repo => repo.ListByHabitIdAsync(It.IsAny<Guid>()), Times.Never);
	}
}
