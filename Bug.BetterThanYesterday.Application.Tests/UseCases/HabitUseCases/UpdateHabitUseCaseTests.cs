using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.Habits.UpdateHabit;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.Strings;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.HabitUseCases;

public class UpdateHabitUseCaseTests : BaseHabitUseCaseTests
{
	[Fact]
	public async Task Test_UpdateHabitUseCase_HabitSuccessfullyUpdated_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UpdateHabitUseCase>();
		var firstHabit = _mock.Habits[0];
		var newName = "Gym";
		var command = new UpdateHabitCommand(firstHabit.Id, newName);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.HabitSuccessfullyUpdated, result.GetMessage());

		var resultData = Assert.IsType<Result<HabitModel>>(result).Data;
		Assert.Equal(firstHabit.Id, resultData.HabitId);
		Assert.Equal(newName, resultData.Name);

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.HabitRepository.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Once);
		_mock.HabitRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Habit>()), Times.Once);
	}

	[Fact]
	public async Task Test_UpdateHabitUseCase_HabitNotFound_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UpdateHabitUseCase>();
		var command = new UpdateHabitCommand(Guid.NewGuid(), "Gym");

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.HabitNotFound, result.GetMessage());

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.HabitRepository.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Never);
		_mock.HabitRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Habit>()), Times.Never);
	}

	[Fact]
	public async Task Test_UpdateHabitUseCase_EnterHabitName_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UpdateHabitUseCase>();
		var firstHabit = _mock.Habits[0];
		var command = new UpdateHabitCommand(firstHabit.Id, string.Empty);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.EnterHabitName, result.GetMessage());

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.HabitRepository.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Never);
		_mock.HabitRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Habit>()), Times.Never);
	}

	[Fact]
	public async Task Test_UpdateHabitUseCase_ThereIsAlreadyAHabitRegisteredWithThatName_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UpdateHabitUseCase>();
		var firstHabit = _mock.Habits[0];
		var otherHabit = _mock.Habits[1];
		var command = new UpdateHabitCommand(firstHabit.Id, otherHabit.Name);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.ThereIsAlreadyAHabitRegisteredWithThatName, result.GetMessage());

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.HabitRepository.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Once);
		_mock.HabitRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Habit>()), Times.Never);
	}

	[Fact]
	public async Task Test_UpdateHabitUseCase_HabitWithSameNameSuccessfullyUpdated_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UpdateHabitUseCase>();
		var firstHabit = _mock.Habits[0];
		var command = new UpdateHabitCommand(firstHabit.Id, firstHabit.Name);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.HabitSuccessfullyUpdated, result.GetMessage());

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.HabitRepository.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Once);
		_mock.HabitRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Habit>()), Times.Once);
	}
}
