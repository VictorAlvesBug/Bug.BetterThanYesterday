using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.Habits.ListAllHabits;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Strings;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.HabitUseCases;

public class ListAllHabitsUseCaseTests : BaseHabitUseCaseTests
{
	[Fact]
	public async Task Test_ListAllHabitsUseCase_Valid_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<ListAllHabitsUseCase>();
		var command = new ListAllHabitsCommand();

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.HabitsSuccessfullyFound, result.GetMessage());

		var resultData = Assert.IsType<Result<IEnumerable<HabitModel>>>(result).Data;
		Assert.Equal(_mock.Habits.Count, resultData.Count());

		_mock.HabitRepository.Verify(repo => repo.ListAllAsync(), Times.Once);
	}
}
