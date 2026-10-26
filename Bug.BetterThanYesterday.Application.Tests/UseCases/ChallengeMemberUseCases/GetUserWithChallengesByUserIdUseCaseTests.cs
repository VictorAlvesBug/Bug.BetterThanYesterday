using Bug.BetterThanYesterday.Application.Mocks;
using Bug.BetterThanYesterday.Application.ChallengeMembers;
using Bug.BetterThanYesterday.Application.ChallengeMembers.GetUserWithChallengesByUserId;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.Strings;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.ChallengeMemberUseCases;

public class GetUserWithChallengesByUserIdUseCaseTests : BaseChallengeMemberUseCaseTests
{
	[Fact]
	public async Task Test_GetUserWithChallengesByUserIdUseCase_UserSuccessfullyFound_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetUserWithChallengesByUserIdUseCase>();
		var userId = _mock.ChallengeMembers.First().UserId;
		var user = _mock.Users.First(user => user.Id == userId);
		var challenges =
			(
				from
					challengeMember in _mock.ChallengeMembers
				where
					challengeMember.UserId == userId
				join challenge in _mock.Challenges 
					on challengeMember.ChallengeId equals challenge.Id
				select
					challenge
			).Distinct()
			.ToList();
		var command = new GetUserWithChallengesByUserIdCommand(userId);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.UserSuccessfullyFound, result.GetMessage());

		var resultData = Assert.IsType<Result<UserWithChallengesModel>>(result).Data;
		Assert.Equal(user.Id, resultData.User.Id);
		Assert.Equal(user.Name, resultData.User.Name);
		Assert.Equal(user.Email.Value, resultData.User.Email);
		Assert.Equal(user.CreatedAt, resultData.User.CreatedAt);
		
		Assert.Equal(challenges.Count, resultData.Challenges.Count);

		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.ListByUserIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeRepository.Verify(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()), Times.Once);
	}

	[Fact]
	public async Task Test_GetUserWithChallengesByUserIdUseCase_UserHasNoChallenges_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetUserWithChallengesByUserIdUseCase>();
		var user = _mock.Users.First(user => user.Id == MockData.UserId6);
		var command = new GetUserWithChallengesByUserIdCommand(user.Id);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.UserHasNoChallenges, result.GetMessage());

		var resultData = Assert.IsType<Result<UserWithChallengesModel>>(result).Data;
		Assert.Equal(user.Id, resultData.User.Id);
		Assert.Equal(user.Name, resultData.User.Name);
		Assert.Equal(user.Email.Value, resultData.User.Email);
		Assert.Equal(user.CreatedAt, resultData.User.CreatedAt);
		
		Assert.Empty(resultData.Challenges);

		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.ListByUserIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeRepository.Verify(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()), Times.Never);
	}

	[Fact]
	public async Task Test_GetUserWithChallengesByUserIdUseCase_UserNotFound_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetUserWithChallengesByUserIdUseCase>();
		var command = new GetUserWithChallengesByUserIdCommand(Guid.NewGuid());

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.UserNotFound, result.GetMessage());

		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.ListByUserIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.ChallengeRepository.Verify(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()), Times.Never);
	}
}
