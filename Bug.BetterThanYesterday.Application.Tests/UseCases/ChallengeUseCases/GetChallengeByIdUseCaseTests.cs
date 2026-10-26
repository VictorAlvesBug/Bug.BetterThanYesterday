using Bug.BetterThanYesterday.Application.Challenges.GetChallengeById;
using Bug.BetterThanYesterday.Application.Challenges;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Moq;
using Xunit;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.ChallengeUseCases;

public class GetChallengeByIdUseCaseTests : BaseChallengeUseCaseTests
{
	[Fact]
	public async Task Test_GetChallengeByIdUseCase_ChallengeSuccessfullyFound_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetChallengeByIdUseCase>();
		var firstChallenge = _mock.Challenges[0];
		var owner = _mock.Users.First(user => user.Id == firstChallenge.OwnerId);
		var command = new GetChallengeByIdCommand(firstChallenge.Id);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.ChallengeSuccessfullyFound, result.GetMessage());

		var resultData = Assert.IsType<Result<ChallengeModel>>(result).Data;

		Assert.Equal(firstChallenge.Id, resultData.Id);
		Assert.Equal(firstChallenge.OwnerId, resultData.OwnerId);
		Assert.Equal(owner.Nickname, resultData.OwnerName);
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
	}

	[Fact]
	public async Task Test_GetChallengeByIdUseCase_ChallengeNotFound_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetChallengeByIdUseCase>();
		var command = new GetChallengeByIdCommand(Guid.NewGuid());

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.ChallengeNotFound, result.GetMessage());

		_mock.ChallengeRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
	}
}
