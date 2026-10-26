using Bug.BetterThanYesterday.Application.Ledger;
using Bug.BetterThanYesterday.Application.Challenges.CancelChallenge;
using Bug.BetterThanYesterday.Application.Challenges;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Moq;
using Xunit;
using Bug.BetterThanYesterday.Domain.Challenges.Entities;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.ChallengeUseCases;

public class CancelChallengeUseCaseTests : BaseChallengeUseCaseTests
{
	[Fact]
	public async Task Test_CancelChallengeUseCase_ChallengeSuccessfullyCancelled_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CancelChallengeUseCase>();
		_mocker.GetMock<IChallengeFinancesProcessor>()
			.Setup(processor => processor.ProcessAsync(It.IsAny<Guid>()))
			.Returns(Task.CompletedTask);
		var firstChallenge = _mock.Challenges[0];
		var command = new CancelChallengeCommand(firstChallenge.Id);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.ChallengeSuccessfullyCancelled, result.GetMessage());

		var resultData = Assert.IsType<Result<ChallengeModel>>(result).Data;

		Assert.Equal(firstChallenge.Id, resultData.Id);
		Assert.Equal(firstChallenge.OwnerId, resultData.OwnerId);
		Assert.Equal(firstChallenge.HabitId, resultData.HabitId);
		Assert.Equal(firstChallenge.Description, resultData.Description);
		Assert.Equal(firstChallenge.StartsAt.ToDateTime(TimeOnly.MinValue), resultData.StartsAt);
		Assert.Equal(firstChallenge.EndsAt.ToDateTime(TimeOnly.MinValue), resultData.EndsAt);
		Assert.Equal(firstChallenge.GetStatus(), ChallengeStatus.Get(resultData.Status));
		Assert.Equal(firstChallenge.Type, ChallengeType.Get(resultData.Type));
		Assert.Equal(firstChallenge.DaysOffPerWeek, resultData.DaysOffPerWeek);
		Assert.Equal(firstChallenge.PenaltyValue, resultData.PenaltyValue);
		Assert.Equal(firstChallenge.CreatedAt, resultData.CreatedAt);

		_mock.ChallengeRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Challenge>()), Times.Once);
	}

	[Fact]
	public async Task Test_CancelChallengeUseCase_ChallengeNotFound_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CancelChallengeUseCase>();
		var command = new CancelChallengeCommand(Guid.NewGuid());

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.ChallengeNotFound, result.GetMessage());

		_mock.ChallengeRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Challenge>()), Times.Never);
	}
}
