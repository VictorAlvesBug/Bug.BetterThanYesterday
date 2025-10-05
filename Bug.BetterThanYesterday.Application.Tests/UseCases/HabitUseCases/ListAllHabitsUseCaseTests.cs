using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.Habits.ListAllHabits;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.HabitUseCases
{
	public class ListAllHabitsUseCaseTests
	{
		private readonly AutoMocker _mocker = new();
		private Mock<IHabitRepository> _habitRepository;
		private readonly List<Habit> _habits;

		public ListAllHabitsUseCaseTests()
		{
			(_habitRepository, _habits) = HabitRepositoryMockFactory.Create();
			_mocker.Use(_habitRepository.Object);
		}

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

			var resultData = Assert.IsType<Result<IEnumerable<HabitModel>>>(result).Data;
			Assert.Equal(_habits.Count, resultData.Count());

			_habitRepository.Verify(repo => repo.ListAllAsync(), Times.Once);
		}
	}
}
