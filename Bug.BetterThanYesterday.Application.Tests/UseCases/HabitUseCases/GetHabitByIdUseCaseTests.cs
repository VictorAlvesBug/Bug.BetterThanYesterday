using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.Habits.GetHabitById;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.HabitUseCases
{
	public class GetHabitByIdUseCaseTests
	{
		private readonly AutoMocker _mocker = new();
		private Mock<IHabitRepository> _habitRepository;
		private readonly List<Habit> _habits;

		public GetHabitByIdUseCaseTests()
		{
			(_habitRepository, _habits) = HabitRepositoryMockFactory.Create();
			_mocker.Use(_habitRepository.Object);
		}

		[Fact]
		public async Task Test_GetHabitByIdUseCase_Valid_ShouldReturnSuccess()
		{
			// Arrange
			var useCase = _mocker.CreateInstance<GetHabitByIdUseCase>();
			var firstHabit = _habits[0];
			var command = new GetHabitByIdCommand(firstHabit.Id);

			// Act
			var result = await useCase.HandleAsync(command);

			// Assert
			Assert.NotNull(result);
			Assert.True(result.IsSuccess());

			var resultData = Assert.IsType<Result<HabitModel>>(result).Data;
			Assert.Equal(firstHabit.Id, resultData.Id);
			Assert.Equal(firstHabit.Name, resultData.Name);
			Assert.Equal(firstHabit.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.CreatedAt);

			_habitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		}

		[Fact]
		public async Task Test_GetHabitByIdUseCase_NotFoundHabitId_ShouldReturnRejected()
		{
			// Arrange
			var useCase = _mocker.CreateInstance<GetHabitByIdUseCase>();
			var command = new GetHabitByIdCommand(Guid.NewGuid());

			// Act
			var result = await useCase.HandleAsync(command);

			// Assert
			Assert.NotNull(result);
			Assert.True(result.IsRejected());

			_habitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		}
	}
}
