using Bug.BetterThanYesterday.Application.Mocks;
using Bug.BetterThanYesterday.Application.ChallengeMembers;
using Bug.BetterThanYesterday.Application.ChallengeMembers.RemoveUserFromChallenge;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.Entities;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.ChallengeMemberUseCases;

public class RemoveUserFromChallengeUseCaseTests : BaseChallengeMemberUseCaseTests
{
	[Fact]
	public async Task Test_RemoveUserFromChallengeUseCase_MemberSuccessfullyRemovedFromTheChallenge_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RemoveUserFromChallengeUseCase>();
		var challenge = _mock.Challenges.First(challenge => challenge.Id == MockData.PrivateNotStartedChallengeId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active);
		var user = _mock.Users.First(user => user.Id == MockData.UserId1);
		var challengeMemberId = ChallengeMember.BuildId(challenge.Id, user.Id);
		var challengeMember = _mock.ChallengeMembers.First(pp => pp.Id == challengeMemberId);
		var command = new RemoveUserFromChallengeCommand(
			challenge.Id,
			user.Id
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.MemberSuccessfullyRemovedFromTheChallenge, result.GetMessage());

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

		_mock.ChallengeRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.DeleteAsync(It.IsAny<ChallengeMember>()), Times.Once);
	}

	[Fact]
	public async Task Test_RemoveUserFromChallengeUseCase_ChallengeNotFound_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RemoveUserFromChallengeUseCase>();
		var command = new RemoveUserFromChallengeCommand(
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
	public async Task Test_RemoveUserFromChallengeUseCase_UserNotFound_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RemoveUserFromChallengeUseCase>();
		var command = new RemoveUserFromChallengeCommand(
			MockData.PublicRunningChallengeId1_WithUserId1Active,
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
	public async Task Test_RemoveUserFromChallengeUseCase_UserIsNotInTheChallenge_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RemoveUserFromChallengeUseCase>();
		var command = new RemoveUserFromChallengeCommand(
			MockData.PrivateNotStartedChallengeId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active,
			MockData.UserId4
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
	public async Task Test_RemoveUserFromChallengeUseCase_BlockedMemberCanLeave_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RemoveUserFromChallengeUseCase>();
		var command = new RemoveUserFromChallengeCommand(
			MockData.PrivateNotStartedChallengeId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active,
			MockData.UserId2
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.MemberSuccessfullyRemovedFromTheChallenge, result.GetMessage());

		_mock.ChallengeMemberRepository.Verify(repo => repo.DeleteAsync(It.IsAny<ChallengeMember>()), Times.Once);
	}

	[Fact]
	public async Task Test_RemoveUserFromChallengeUseCase_UserCannotBeRemovedFromFinishedChallenge_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RemoveUserFromChallengeUseCase>();
		var command = new RemoveUserFromChallengeCommand(
			MockData.PrivateFinishedChallengeId5_WithUserId5Active,
			MockData.UserId5
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.MemberCanOnlyBeRemovedFromNotStartedOrRunningChallenges, result.GetMessage());

		_mock.ChallengeRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ChallengeMember>()), Times.Never);
	}

	[Fact]
	public async Task Test_RemoveUserFromChallengeUseCase_UserCannotBeRemovedFromCancelledChallenge_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RemoveUserFromChallengeUseCase>();
		var command = new RemoveUserFromChallengeCommand(
			MockData.PublicCancelledChallengeId6_WithUserId4BlockedAndUserId5Active,
			MockData.UserId5
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.MemberCanOnlyBeRemovedFromNotStartedOrRunningChallenges, result.GetMessage());

		_mock.ChallengeRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ChallengeMember>()), Times.Never);
	}
}
