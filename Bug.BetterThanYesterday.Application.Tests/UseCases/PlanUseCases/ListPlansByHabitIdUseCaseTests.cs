using Bug.BetterThanYesterday.Application.Plans.ListPlansByHabitId;
using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.PlanUseCases;

public class ListPlansByHabitIdUseCaseTests : BasePlanUseCaseTests
{
	[Fact]
	public async Task Test_ListPlansByHabitIdUseCase_Valid_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<ListPlansByHabitIdUseCase>();
		var studyingHabit = _mock.Habits.First(h => h.Name == "Studying");
		var command = new ListPlansByHabitIdCommand(studyingHabit.Id);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());

		var resultData = Assert.IsType<Result<IEnumerable<PlanModel>>>(result).Data;
		Assert.Equal(_mock.Plans.Count(plan => plan.HabitId == studyingHabit.Id), resultData.Count());

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanRepository.Verify(repo => repo.ListByHabitIdAsync(It.IsAny<Guid>()), Times.Once);
	}

	[Fact]
	public async Task Test_ListPlansByHabitIdUseCase_NotFoundHabitId_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<ListPlansByHabitIdUseCase>();
		var command = new ListPlansByHabitIdCommand(Guid.NewGuid());

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanRepository.Verify(repo => repo.ListByHabitIdAsync(It.IsAny<Guid>()), Times.Never);
	}
}
