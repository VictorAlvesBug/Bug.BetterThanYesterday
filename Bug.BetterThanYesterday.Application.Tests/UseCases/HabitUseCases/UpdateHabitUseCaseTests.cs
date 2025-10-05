using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Application.Habits.UpdateHabit;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.Habits;
using Moq.AutoMock;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.HabitUseCases
{
	public class UpdateHabitUseCaseTests
	{
		private readonly AutoMocker _mocker = new();
		private readonly Mock<IHabitRepository> _habitRepository;
		private readonly List<Habit> _habits;

		public UpdateHabitUseCaseTests()
		{
			(_habitRepository, _habits) = HabitRepositoryMockFactory.Create();
			_mocker.Use(_habitRepository.Object);
		}

		[Fact]
		public async Task Test_UpdateHabitUseCase_Valid_ShouldReturnSuccess()
		{
			// Arrange
			var useCase = _mocker.CreateInstance<UpdateHabitUseCase>();
			var firstHabit = _habits[0];
			var command = new UpdateHabitCommand(firstHabit.Id, "Gym");

			// Act
			var result = await useCase.HandleAsync(command);

			// Assert
			Assert.NotNull(result);
			Assert.True(result.IsSuccess());

			_habitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
			_habitRepository.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Once);
			_habitRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Habit>()), Times.Once);
		}

		[Fact]
		public async Task Test_UpdateHabitUseCase_NotFoundHabitId_ShouldReturnRejected()
		{
			// Arrange
			var useCase = _mocker.CreateInstance<UpdateHabitUseCase>();
			var command = new UpdateHabitCommand(Guid.NewGuid(), "Gym");

			// Act
			var result = await useCase.HandleAsync(command);

			// Assert
			Assert.NotNull(result);
			Assert.True(result.IsRejected());

			_habitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
			_habitRepository.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Never);
			_habitRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Habit>()), Times.Never);
		}

		[Fact]
		public async Task Test_UpdateHabitUseCase_EmptyName_ShouldReturnRejected()
		{
			// Arrange
			var useCase = _mocker.CreateInstance<UpdateHabitUseCase>();
			var firstHabit = _habits[0];
			var command = new UpdateHabitCommand(firstHabit.Id, "");

			// Act & Assert
			await Assert.ThrowsAsync<ArgumentNullException>(async () => await useCase.HandleAsync(command));

			_habitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
			_habitRepository.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Never);
			_habitRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Habit>()), Times.Never);
		}

		[Fact]
		public async Task Test_UpdateHabitUseCase_SameNameAndOtherId_ShouldReturnRejected()
		{
			// Arrange
			var useCase = _mocker.CreateInstance<UpdateHabitUseCase>();
			var firstHabit = _habits[0];
			var otherHabit = _habits[1];
			var command = new UpdateHabitCommand(firstHabit.Id, otherHabit.Name);

			// Act
			var result = await useCase.HandleAsync(command);

			// Assert
			Assert.NotNull(result);
			Assert.True(result.IsRejected());

			_habitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
			_habitRepository.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Once);
			_habitRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Habit>()), Times.Never);
		}

		[Fact]
		public async Task Test_UpdateHabitUseCase_SameNameAndSameId_ShouldReturnSuccess()
		{
			// Arrange
			var useCase = _mocker.CreateInstance<UpdateHabitUseCase>();
			var firstHabit = _habits[0];
			var command = new UpdateHabitCommand(firstHabit.Id, firstHabit.Name);

			// Act
			var result = await useCase.HandleAsync(command);

			// Assert
			Assert.NotNull(result);
			Assert.True(result.IsSuccess());

			_habitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
			_habitRepository.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Once);
			_habitRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Habit>()), Times.Once);
		}
	}
}
