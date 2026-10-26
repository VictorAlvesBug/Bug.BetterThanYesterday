using Bug.BetterThanYesterday.Application.Mocks;
using Bug.BetterThanYesterday.Application.ChallengeMembers;
using Bug.BetterThanYesterday.Application.ChallengeMembers.AddUserToChallenge;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.Entities;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.ChallengeMemberUseCases;

public class AddUserToChallengeUseCaseTests : BaseChallengeMemberUseCaseTests
{
	private readonly DateTime _today = DateTime.Today;

	[Fact]
	public async Task Test_AddUserToChallengeUseCase_MemberSuccessfullyAddedToTheChallenge_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<AddUserToChallengeUseCase>();
		var challenge = _mock.Challenges.First(challenge => challenge.Id == MockData.PrivateNotStartedChallengeId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active);
		var user = _mock.Users.First(user => user.Id == MockData.UserId4);
		var command = new AddUserToChallengeCommand(
			challenge.Id,
			user.Id
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.MemberSuccessfullyAddedToTheChallenge, result.GetMessage());

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

		_mock.ChallengeRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.AddAsync(It.IsAny<ChallengeMember>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ChallengeMember>()), Times.Never);
	}

	[Fact]
	public async Task Test_AddUserToChallengeUseCase_ChallengeNotFound_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<AddUserToChallengeUseCase>();
		var user = _mock.Users.First(user => user.Id == MockData.UserId3);
		var command = new AddUserToChallengeCommand(
			Guid.NewGuid(),
			user.Id
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
		_mock.ChallengeMemberRepository.Verify(repo => repo.AddAsync(It.IsAny<ChallengeMember>()), Times.Never);
		_mock.ChallengeMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ChallengeMember>()), Times.Never);
	}

	[Fact]
	public async Task Test_AddUserToChallengeUseCase_UserNotFound_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<AddUserToChallengeUseCase>();
		var challenge = _mock.Challenges.First(challenge => challenge.Id == MockData.PublicCancelledChallengeId3);
		var command = new AddUserToChallengeCommand(
			challenge.Id,
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
		_mock.ChallengeMemberRepository.Verify(repo => repo.AddAsync(It.IsAny<ChallengeMember>()), Times.Never);
		_mock.ChallengeMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ChallengeMember>()), Times.Never);
	}

	[Fact]
	public async Task Test_AddUserToChallengeUseCase_ActiveMemberAlreadyAddedToTheChallenge_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<AddUserToChallengeUseCase>();
		var challenge = _mock.Challenges.First(challenge => challenge.Id == MockData.PrivateNotStartedChallengeId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active);
		var user = _mock.Users.First(user => user.Id == MockData.UserId1);
		var command = new AddUserToChallengeCommand(
			challenge.Id,
			user.Id
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.MemberAlreadyAddedToTheChallenge, result.GetMessage());

		_mock.ChallengeRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.AddAsync(It.IsAny<ChallengeMember>()), Times.Never);
		_mock.ChallengeMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ChallengeMember>()), Times.Never);
	}

	[Fact]
	public async Task Test_AddUserToChallengeUseCase_BlockedMemberAlreadyAddedToTheChallenge_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<AddUserToChallengeUseCase>();
		var challenge = _mock.Challenges.First(challenge => challenge.Id == MockData.PrivateNotStartedChallengeId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active);
		var user = _mock.Users.First(user => user.Id == MockData.UserId2);
		var command = new AddUserToChallengeCommand(
			challenge.Id,
			user.Id
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.MemberAlreadyAddedToTheChallenge, result.GetMessage());

		_mock.ChallengeRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.AddAsync(It.IsAny<ChallengeMember>()), Times.Never);
		_mock.ChallengeMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ChallengeMember>()), Times.Never);
	}

	[Fact]
	public async Task Test_AddUserToChallengeUseCase_MemberSuccessfullyReaddedToTheChallenge_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<AddUserToChallengeUseCase>();
		var challenge = _mock.Challenges.First(challenge => challenge.Id == MockData.PrivateNotStartedChallengeId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active);
		var user = _mock.Users.First(user => user.Id == MockData.UserId3);
		var command = new AddUserToChallengeCommand(
			challenge.Id,
			user.Id
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.MemberSuccessfullyReaddedToTheChallenge, result.GetMessage());

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

		_mock.ChallengeRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.AddAsync(It.IsAny<ChallengeMember>()), Times.Never);
		_mock.ChallengeMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ChallengeMember>()), Times.Once);
	}

	[Fact]
	public async Task Test_AddUserToChallengeUseCase_UserCannotBeAddedToRunningChallenge_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<AddUserToChallengeUseCase>();
		var challenge = _mock.Challenges.First(challenge => challenge.Id == MockData.PublicRunningChallengeId1_WithUserId1Active);
		var user = _mock.Users.First(user => user.Id == MockData.UserId3);
		var command = new AddUserToChallengeCommand(
			challenge.Id,
			user.Id
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.OnlyNotStartedChallengesCanReceiveNewMembers, result.GetMessage());

		_mock.ChallengeRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.ChallengeMemberRepository.Verify(repo => repo.AddAsync(It.IsAny<ChallengeMember>()), Times.Never);
		_mock.ChallengeMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ChallengeMember>()), Times.Never);
	}

	[Fact]
	public async Task Test_AddUserToChallengeUseCase_UserCannotBeAddedToFinishedChallenge_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<AddUserToChallengeUseCase>();
		var challenge = _mock.Challenges.First(challenge => challenge.Id == MockData.PrivateFinishedChallengeId4_WithUserId2ActiveAndUserId3Blocked);
		var user = _mock.Users.First(user => user.Id == MockData.UserId1);
		var command = new AddUserToChallengeCommand(
			challenge.Id,
			user.Id
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.OnlyNotStartedChallengesCanReceiveNewMembers, result.GetMessage());

		_mock.ChallengeRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.ChallengeMemberRepository.Verify(repo => repo.AddAsync(It.IsAny<ChallengeMember>()), Times.Never);
		_mock.ChallengeMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ChallengeMember>()), Times.Never);
	}

	[Fact]
	public async Task Test_AddUserToChallengeUseCase_UserCannotBeAddedToCancelledChallenge_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<AddUserToChallengeUseCase>();
		var challenge = _mock.Challenges.First(challenge => challenge.Id == MockData.PublicCancelledChallengeId3);
		var user = _mock.Users.First(user => user.Id == MockData.UserId1);
		var command = new AddUserToChallengeCommand(
			challenge.Id,
			user.Id
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.OnlyNotStartedChallengesCanReceiveNewMembers, result.GetMessage());

		_mock.ChallengeRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.ChallengeMemberRepository.Verify(repo => repo.AddAsync(It.IsAny<ChallengeMember>()), Times.Never);
		_mock.ChallengeMemberRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ChallengeMember>()), Times.Never);
	}
}
