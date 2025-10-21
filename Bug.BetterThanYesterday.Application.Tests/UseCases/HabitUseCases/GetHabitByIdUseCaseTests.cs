using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.Habits.GetHabitById;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Strings;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.HabitUseCases;

public class GetHabitByIdUseCaseTests : BaseHabitUseCaseTests
{
	[Fact]
	public async Task Test_GetHabitByIdUseCase_HabitSuccessfullyFound_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetHabitByIdUseCase>();
		var firstHabit = _mock.Habits[0];
		var command = new GetHabitByIdCommand(firstHabit.Id);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.HabitSuccessfullyFound, result.GetMessage());

		var resultData = Assert.IsType<Result<HabitModel>>(result).Data;
		Assert.Equal(firstHabit.Id, resultData.HabitId);
		Assert.Equal(firstHabit.Name, resultData.Name);
		Assert.Equal(firstHabit.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.CreatedAt);

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
	}

	[Fact]
	public async Task Test_GetHabitByIdUseCase_HabitNotFound_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetHabitByIdUseCase>();
		var command = new GetHabitByIdCommand(Guid.NewGuid());

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.HabitNotFound, result.GetMessage());

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
	}
}
