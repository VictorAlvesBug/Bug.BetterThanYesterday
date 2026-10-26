using Bug.BetterThanYesterday.Application.ChallengeMembers;
using Bug.BetterThanYesterday.Application.ChallengeMembers.GetChallengeMemberDetails;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.ChallengeMemberUseCases;

public class GetChallengeMemberDetailsUseCaseTests : BaseChallengeMemberUseCaseTests
{
	[Fact]
	public async Task Test_GetChallengeMemberDetailsUseCase_ChallengeMemberSuccessfullyFound_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetChallengeMemberDetailsUseCase>();
		var firstChallengeMember = _mock.ChallengeMembers[0];
		var challenge = _mock.Challenges.First(challenge => challenge.Id == firstChallengeMember.ChallengeId);
		var user = _mock.Users.First(user => user.Id == firstChallengeMember.UserId);
		var command = new GetChallengeMemberDetailsCommand(
			firstChallengeMember.ChallengeId,
			firstChallengeMember.UserId
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.ChallengeMemberSuccessfullyFound, result.GetMessage());

		var resultData = Assert.IsType<Result<ChallengeMemberDetailsModel>>(result).Data;
		Assert.Equal(firstChallengeMember.Id, resultData.Id);
		Assert.Equal(firstChallengeMember.JoinedAt.ToDateTime(TimeOnly.MinValue), resultData.JoinedAt);
		Assert.Equal(firstChallengeMember.Status.Name, resultData.Status);
		
		Assert.Equal(challenge.Id, resultData.Challenge.Id);
		Assert.Equal(challenge.OwnerId, resultData.Challenge.OwnerId);
		Assert.Equal(challenge.HabitId, resultData.Challenge.HabitId);
		Assert.Equal(challenge.Description, resultData.Challenge.Description);
		Assert.Equal(challenge.StartsAt.ToDateTime(TimeOnly.MinValue), resultData.Challenge.StartsAt);
		Assert.Equal(challenge.EndsAt.ToDateTime(TimeOnly.MinValue), resultData.Challenge.EndsAt);
		Assert.Equal(challenge.GetStatus(), ChallengeStatus.Get(resultData.Challenge.Status));
		Assert.Equal(challenge.Type, ChallengeType.Get(resultData.Challenge.Type));
		Assert.Equal(challenge.DaysOffPerWeek, resultData.Challenge.DaysOffPerWeek);
		Assert.Equal(challenge.PenaltyValue, resultData.Challenge.PenaltyValue);
		Assert.Equal(challenge.CreatedAt, resultData.Challenge.CreatedAt);
		
		Assert.Equal(user.Id, resultData.User.Id);
		Assert.Equal(user.Name, resultData.User.Name);
		Assert.Equal(user.Email.Value, resultData.User.Email);
		Assert.Equal(user.CreatedAt, resultData.User.CreatedAt);

		_mock.ChallengeMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
	}

	[Fact]
	public async Task Test_GetChallengeMemberDetailsUseCase_ChallengeMemberNotFound_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetChallengeMemberDetailsUseCase>();
		var firstChallengeMember = _mock.ChallengeMembers[0];
		var command = new GetChallengeMemberDetailsCommand(
			Guid.NewGuid(),
			Guid.NewGuid()
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.ChallengeMemberNotFound, result.GetMessage());

		_mock.ChallengeMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
	}
}
