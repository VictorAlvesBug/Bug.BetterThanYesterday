using Bug.BetterThanYesterday.Application.Challenges.ListChallengesByFilter;
using Bug.BetterThanYesterday.Application.Challenges;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Moq;
using Xunit;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.ChallengeUseCases;

public class ListChallengesByFilterUseCaseTests : BaseChallengeUseCaseTests
{
	[Fact]
	public async Task Test_ListChallengesByFilterUseCase_ChallengesSuccessfullyFound_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<ListChallengesByFilterUseCase>();
		var command = new ListChallengesByFilterCommand();

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.ChallengesSuccessfullyFound, result.GetMessage());

		var resultData = Assert.IsType<Result<IEnumerable<ChallengeModel>>>(result).Data;
		Assert.Equal(_mock.Challenges.Count, resultData.Count());

		_mock.ChallengeRepository.Verify(repo => repo.ListAllAsync(), Times.Once);
	}
}
