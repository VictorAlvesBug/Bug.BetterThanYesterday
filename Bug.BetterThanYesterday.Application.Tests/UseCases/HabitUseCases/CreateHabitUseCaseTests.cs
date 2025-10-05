using Bug.BetterThanYesterday.Application.Habits.CreateHabit;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.Habits;
using Moq.AutoMock;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.HabitUseCases
{
	public class CreateHabitUseCaseTests
	{
		private readonly AutoMocker _mocker = new();
		private Mock<IHabitRepository> _habitRepository;
		private readonly List<Habit> _habits;

		public CreateHabitUseCaseTests()
		{
			(_habitRepository, _habits) = HabitRepositoryMockFactory.Create();
			_mocker.Use(_habitRepository.Object);
		}

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

			_habitRepository.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Once);
			_habitRepository.Verify(repo => repo.AddAsync(It.IsAny<Habit>()), Times.Once);
		}

		[Fact]
		public async Task Test_CreateHabitUseCase_EmptyName_ShouldReturnRejected()
		{
			// Arrange
			var useCase = _mocker.CreateInstance<CreateHabitUseCase>();
			var command = new CreateHabitCommand("");

			// Act & Assert
			await Assert.ThrowsAsync<ArgumentNullException>(async () => await useCase.HandleAsync(command));

			_habitRepository.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Never);
			_habitRepository.Verify(repo => repo.AddAsync(It.IsAny<Habit>()), Times.Never);
		}

		[Fact]
		public async Task Test_CreateHabitUseCase_DuplicatedName_ShouldReturnRejected()
		{
			// Arrange
			var useCase = _mocker.CreateInstance<CreateHabitUseCase>();
			var firstHabit = _habits[0];
			var command = new CreateHabitCommand(firstHabit.Name);

			// Act
			var result = await useCase.HandleAsync(command);

			// Assert
			Assert.NotNull(result);
			Assert.True(result.IsRejected());

			_habitRepository.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Once);
			_habitRepository.Verify(repo => repo.AddAsync(It.IsAny<Habit>()), Times.Never);
		}
	}
}
