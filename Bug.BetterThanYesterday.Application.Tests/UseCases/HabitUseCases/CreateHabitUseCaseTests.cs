using Bug.BetterThanYesterday.Application.Habits.CreateHabit;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.HabitUseCases;

public class CreateHabitUseCaseTests : BaseHabitUseCaseTests
{
	[Fact]
	public async Task Test_CreateHabitUseCase_Valid_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CreateHabitUseCase>();
		var command = new CreateHabitCommand("Health Eating");

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());

		_mock.HabitRepository.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Once);
		_mock.HabitRepository.Verify(repo => repo.AddAsync(It.IsAny<Habit>()), Times.Once);
	}

	[Fact]
	public async Task Test_CreateHabitUseCase_EmptyName_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CreateHabitUseCase>();
		var command = new CreateHabitCommand("");

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentNullException>(async () => await useCase.HandleAsync(command));

		_mock.HabitRepository.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Never);
		_mock.HabitRepository.Verify(repo => repo.AddAsync(It.IsAny<Habit>()), Times.Never);
	}

	[Fact]
	public async Task Test_CreateHabitUseCase_DuplicatedName_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CreateHabitUseCase>();
		var firstHabit = _mock.Habits[0];
		var command = new CreateHabitCommand(firstHabit.Name);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		_mock.HabitRepository.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Once);
		_mock.HabitRepository.Verify(repo => repo.AddAsync(It.IsAny<Habit>()), Times.Never);
	}
}
