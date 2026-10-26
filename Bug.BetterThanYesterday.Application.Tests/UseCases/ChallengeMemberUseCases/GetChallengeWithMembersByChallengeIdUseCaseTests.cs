using Bug.BetterThanYesterday.Application.Mocks;
using Bug.BetterThanYesterday.Application.ChallengeMembers;
using Bug.BetterThanYesterday.Application.ChallengeMembers.GetChallengeWithMembersByChallengeId;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.ChallengeMemberUseCases;

public class GetChallengeWithMembersByChallengeIdUseCaseTests : BaseChallengeMemberUseCaseTests
{
	[Fact]
	public async Task Test_GetChallengeWithMembersByChallengeIdUseCase_ChallengeSuccessfullyFound_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetChallengeWithMembersByChallengeIdUseCase>();
		var challengeId = _mock.ChallengeMembers.First().ChallengeId;
		var challenge = _mock.Challenges.First(challenge => challenge.Id == challengeId);
		var owner = _mock.Users.First(user => user.Id == challenge.OwnerId);
		var users =
			(
				from
					challengeMember in _mock.ChallengeMembers
				where
					challengeMember.ChallengeId == challengeId
				join user in _mock.Users 
					on challengeMember.UserId equals user.Id
				select
					user
			).Distinct()
			.ToList();
		var command = new GetChallengeWithMembersByChallengeIdCommand(challengeId);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.ChallengeSuccessfullyFound, result.GetMessage());

		var resultData = Assert.IsType<Result<ChallengeWithMembersModel>>(result).Data;

		Assert.Equal(challenge.Id, resultData.Challenge.Id);
		Assert.Equal(challenge.OwnerId, resultData.Challenge.OwnerId);
		Assert.Equal(owner.Nickname, resultData.Challenge.OwnerName);
		Assert.Equal(challenge.HabitId, resultData.Challenge.HabitId);
		Assert.Equal(challenge.Description, resultData.Challenge.Description);
		Assert.Equal(challenge.StartsAt.ToDateTime(TimeOnly.MinValue), resultData.Challenge.StartsAt);
		Assert.Equal(challenge.EndsAt.ToDateTime(TimeOnly.MinValue), resultData.Challenge.EndsAt);
		Assert.Equal(challenge.GetStatus(), ChallengeStatus.Get(resultData.Challenge.Status));
		Assert.Equal(challenge.Type, ChallengeType.Get(resultData.Challenge.Type));
		Assert.Equal(challenge.DaysOffPerWeek, resultData.Challenge.DaysOffPerWeek);
		Assert.Equal(challenge.PenaltyValue, resultData.Challenge.PenaltyValue);
		Assert.Equal(challenge.CreatedAt, resultData.Challenge.CreatedAt);
		
		Assert.Equal(users.Count, resultData.Users.Count);

		_mock.ChallengeRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.ListByChallengeIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()), Times.Once);
	}

	[Fact]
	public async Task Test_GetChallengeWithMembersByChallengeIdUseCase_ChallengeHasNoMembers_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetChallengeWithMembersByChallengeIdUseCase>();
		var challenge = _mock.Challenges.First(challenge => challenge.Id == MockData.PublicCancelledChallengeId3);
		var owner = _mock.Users.First(user => user.Id == challenge.OwnerId);
		var command = new GetChallengeWithMembersByChallengeIdCommand(challenge.Id);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.ChallengeHasNoMembers, result.GetMessage());

		var resultData = Assert.IsType<Result<ChallengeWithMembersModel>>(result).Data;

		Assert.Equal(challenge.Id, resultData.Challenge.Id);
		Assert.Equal(challenge.OwnerId, resultData.Challenge.OwnerId);
		Assert.Equal(owner.Nickname, resultData.Challenge.OwnerName);
		Assert.Equal(challenge.HabitId, resultData.Challenge.HabitId);
		Assert.Equal(challenge.Description, resultData.Challenge.Description);
		Assert.Equal(challenge.StartsAt.ToDateTime(TimeOnly.MinValue), resultData.Challenge.StartsAt);
		Assert.Equal(challenge.EndsAt.ToDateTime(TimeOnly.MinValue), resultData.Challenge.EndsAt);
		Assert.Equal(challenge.GetStatus(), ChallengeStatus.Get(resultData.Challenge.Status));
		Assert.Equal(challenge.Type, ChallengeType.Get(resultData.Challenge.Type));
		Assert.Equal(challenge.DaysOffPerWeek, resultData.Challenge.DaysOffPerWeek);
		Assert.Equal(challenge.PenaltyValue, resultData.Challenge.PenaltyValue);
		Assert.Equal(challenge.CreatedAt, resultData.Challenge.CreatedAt);
		
		Assert.Empty(resultData.Users);

		_mock.ChallengeRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.ListByChallengeIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()), Times.Never);
	}

	[Fact]
	public async Task Test_GetChallengeWithMembersByChallengeIdUseCase_ChallengeNotFound_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetChallengeWithMembersByChallengeIdUseCase>();
		var command = new GetChallengeWithMembersByChallengeIdCommand(Guid.NewGuid());

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.ChallengeNotFound, result.GetMessage());

		_mock.ChallengeRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.ListByChallengeIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.UserRepository.Verify(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()), Times.Never);
	}
}
