using Bug.BetterThanYesterday.Application.Habits.DeleteHabit;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.Habits.Policies;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.HabitUseCases
{
	public class DeleteHabitUseCaseTests
	{
		private readonly AutoMocker _mocker;
		private readonly Mock<IHabitRepository> _habitRepository;
		private readonly Mock<IPlanRepository> _planRepository;
		private readonly List<Habit> _habits;

		public DeleteHabitUseCaseTests()
		{
			_mocker = new();

			(_habitRepository, _habits) = HabitRepositoryMockFactory.Create();
			_mocker.Use(_habitRepository.Object);

			(_planRepository, _) = PlanRepositoryMockFactory.Create();
			_mocker.Use(_planRepository.Object);

			var habitDeletionPolicy = new HabitDeletionPolicy(_planRepository.Object);
			_mocker.Use(habitDeletionPolicy);
		}

		[Fact]
		public async Task Test_DeleteHabitUseCase_Valid_ShouldReturnSuccess()
		{
			// Arrange
			var useCase = _mocker.CreateInstance<DeleteHabitUseCase>();
			var cookingHabit = _habits.First(habit => habit.Name == "Cooking");
			var command = new DeleteHabitCommand(cookingHabit.Id);

			// Act
			var result = await useCase.HandleAsync(command);

			// Assert
			Assert.NotNull(result);
			Assert.True(result.IsSuccess());

			_habitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
			_planRepository.Verify(repo => repo.ListByHabitIdAsync(It.IsAny<Guid>()), Times.Once);
			_habitRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Habit>()), Times.Once);
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

			_habitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
			_planRepository.Verify(repo => repo.ListByHabitIdAsync(It.IsAny<Guid>()), Times.Never);
			_habitRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Habit>()), Times.Never);
		}

		[Fact]
		public async Task Test_DeleteHabitUseCase_NonCancelledPlansRelated_ShouldReturnRejected()
		{
			// Arrange
			var useCase = _mocker.CreateInstance<DeleteHabitUseCase>();
			var studingHabit = _habits.First(habit => habit.Name == "Studing");
			var command = new DeleteHabitCommand(studingHabit.Id);

			// Act
			var result = await useCase.HandleAsync(command);

			// Assert
			Assert.NotNull(result);
			Assert.True(result.IsRejected());

			_habitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
			_planRepository.Verify(repo => repo.ListByHabitIdAsync(It.IsAny<Guid>()), Times.Once);
			_habitRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Habit>()), Times.Never);
		}
	}
}
