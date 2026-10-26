using Bug.BetterThanYesterday.Application.Mocks;
using Bug.BetterThanYesterday.Application.ChallengeMembers;
using Bug.BetterThanYesterday.Application.ChallengeMembers.BlockUserInTheChallenge;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.Entities;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.ChallengeMemberUseCases;

public class BlockUserInTheChallengeUseCaseTests : BaseChallengeMemberUseCaseTests
{
	[Fact]
	public async Task Test_BlockUserInTheChallengeUseCase_MemberSuccessfullyBlocked_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<BlockUserInTheChallengeUseCase>();
		var challenge = _mock.Challenges.First(challenge => challenge.Id == MockData.PublicRunningChallengeId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active);
		var user = _mock.Users.First(user => user.Id == MockData.UserId5);
		var challengeMemberId = ChallengeMember.BuildId(challenge.Id, user.Id);
		var challengeMember = _mock.ChallengeMembers.First(pp => pp.Id == challengeMemberId);
		var command = new BlockUserInTheChallengeCommand(
			challenge.Id,
			user.Id
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.MemberSuccessfullyBlocked, result.GetMessage());

		var resultData = Assert.IsType<Result<ChallengeMemberDetailsModel>>(result).Data;
		
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

		Assert.Equal(challengeMember.Id, resultData.Id);
		Assert.Equal(challengeMember.JoinedAt.ToDateTime(TimeOnly.MinValue), resultData.JoinedAt);
		Assert.Equal(ChallengeMemberStatus.Blocked.Name, resultData.Status);

		_mock.ChallengeRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ChallengeMember>()), Times.Once);
	}

	[Fact]
	public async Task Test_BlockUserInTheChallengeUseCase_ChallengeNotFound_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<BlockUserInTheChallengeUseCase>();
		var command = new BlockUserInTheChallengeCommand(
			Guid.NewGuid(),
			MockData.UserId1
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.ChallengeNotFound, result.GetMessage());

		_mock.ChallengeRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.ChallengeMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.ChallengeMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ChallengeMember>()), Times.Never);
	}

	[Fact]
	public async Task Test_BlockUserInTheChallengeUseCase_UserNotFound_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<BlockUserInTheChallengeUseCase>();
		var command = new BlockUserInTheChallengeCommand(
			MockData.PublicRunningChallengeId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active,
			Guid.NewGuid()
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.UserNotFound, result.GetMessage());

		_mock.ChallengeRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.ChallengeMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ChallengeMember>()), Times.Never);
	}

	[Fact]
	public async Task Test_BlockUserInTheChallengeUseCase_UserIsNotInTheChallenge_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<BlockUserInTheChallengeUseCase>();
		var command = new BlockUserInTheChallengeCommand(
			MockData.PublicRunningChallengeId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active,
			MockData.UserId2
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.UserIsNotInTheChallenge, result.GetMessage());

		_mock.ChallengeRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ChallengeMember>()), Times.Never);
	}

	[Fact]
	public async Task Test_BlockUserInTheChallengeUseCase_MemberAlreadyBlockedInThisChallenge_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<BlockUserInTheChallengeUseCase>();
		var command = new BlockUserInTheChallengeCommand(
			MockData.PublicRunningChallengeId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active,
			MockData.UserId3
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.MemberAlreadyBlockedInThisChallenge, result.GetMessage());

		_mock.ChallengeRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ChallengeMember>()), Times.Never);
	}

	[Fact]
	public async Task Test_BlockUserInTheChallengeUseCase_UserCannotBeBlockedInNotStartedChallenge_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<BlockUserInTheChallengeUseCase>();
		var command = new BlockUserInTheChallengeCommand(
			MockData.PrivateNotStartedChallengeId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active,
			MockData.UserId1
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.MemberCanOnlyBeBlockedInRunningChallenges, result.GetMessage());

		_mock.ChallengeRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ChallengeMember>()), Times.Never);
	}

	[Fact]
	public async Task Test_BlockUserInTheChallengeUseCase_UserCannotBeBlockedInFinishedChallenge_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<BlockUserInTheChallengeUseCase>();
		var command = new BlockUserInTheChallengeCommand(
			MockData.PrivateFinishedChallengeId5_WithUserId5Active,
			MockData.UserId5
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.MemberCanOnlyBeBlockedInRunningChallenges, result.GetMessage());

		_mock.ChallengeRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ChallengeMember>()), Times.Never);
	}

	[Fact]
	public async Task Test_BlockUserInTheChallengeUseCase_UserCannotBeBlockedInCancelledChallenge_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<BlockUserInTheChallengeUseCase>();
		var command = new BlockUserInTheChallengeCommand(
			MockData.PublicCancelledChallengeId6_WithUserId4BlockedAndUserId5Active,
			MockData.UserId5
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.MemberCanOnlyBeBlockedInRunningChallenges, result.GetMessage());

		_mock.ChallengeRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ChallengeMember>()), Times.Never);
	}
}
