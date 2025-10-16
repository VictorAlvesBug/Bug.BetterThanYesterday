using Bug.BetterThanYesterday.Application.Habits.DeleteHabit;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.HabitUseCases;

public class DeleteHabitUseCaseTests : BaseHabitUseCaseTests
{
	[Fact]
	public async Task Test_DeleteHabitUseCase_Valid_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<DeleteHabitUseCase>();
		var habit = _mock.Habits.First(habit => habit.Id == HabitRepositoryMockFactory.HabitId4);
		var command = new DeleteHabitCommand(habit.Id);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanRepository.Verify(repo => repo.ListByHabitIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.HabitRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Habit>()), Times.Once);
	}

	[Fact]
	public async Task Test_DeleteHabitUseCase_NotFoundHabitId_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<DeleteHabitUseCase>();
		var command = new DeleteHabitCommand(Guid.NewGuid());

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanRepository.Verify(repo => repo.ListByHabitIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.HabitRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Habit>()), Times.Never);
	}

	[Fact]
	public async Task Test_DeleteHabitUseCase_NonCancelledPlansRelated_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<DeleteHabitUseCase>();
		var habit = _mock.Habits.First(habit => habit.Id == HabitRepositoryMockFactory.HabitId3);
		var command = new DeleteHabitCommand(habit.Id);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanRepository.Verify(repo => repo.ListByHabitIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.HabitRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Habit>()), Times.Never);
	}
}
