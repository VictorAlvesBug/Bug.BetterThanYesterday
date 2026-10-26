using Bug.BetterThanYesterday.Application.Challenges.ListChallengesByHabitId;
using Bug.BetterThanYesterday.Application.Challenges;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Moq;
using Xunit;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Application.Mocks;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.ChallengeUseCases;

public class ListChallengesByHabitIdUseCaseTests : BaseChallengeUseCaseTests
{
	[Fact]
	public async Task Test_ListChallengesByHabitIdUseCase_ChallengesSuccessfullyFound_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<ListChallengesByHabitIdUseCase>();
		var habit = _mock.Habits.First(h => h.Id == MockData.HabitId3);
		var command = new ListChallengesByHabitIdCommand(habit.Id);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.ChallengesSuccessfullyFound, result.GetMessage());

		var resultData = Assert.IsType<Result<IEnumerable<ChallengeModel>>>(result).Data;
		Assert.Equal(_mock.Challenges.Count(challenge => challenge.HabitId == habit.Id), resultData.Count());

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeRepository.Verify(repo => repo.ListByHabitIdAsync(It.IsAny<Guid>()), Times.Once);
	}

	[Fact]
	public async Task Test_ListChallengesByHabitIdUseCase_HabitNotFound_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<ListChallengesByHabitIdUseCase>();
		var command = new ListChallengesByHabitIdCommand(Guid.NewGuid());

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.HabitNotFound, result.GetMessage());

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeRepository.Verify(repo => repo.ListByHabitIdAsync(It.IsAny<Guid>()), Times.Never);
	}
}
