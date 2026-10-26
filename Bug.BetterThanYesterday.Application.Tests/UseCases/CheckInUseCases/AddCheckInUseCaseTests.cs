using Bug.BetterThanYesterday.Application.CheckIns;
using Bug.BetterThanYesterday.Application.CheckIns.AddCheckIn;
using Bug.BetterThanYesterday.Application.CheckIns.Models;
using Bug.BetterThanYesterday.Application.Mocks;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.ChallengeMembers;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.CheckInUseCases;

public class AddCheckInUseCaseTests : BaseCheckInUseCaseTests
{
    [Fact]
    public async Task Test_AddCheckInUseCase_CheckInSuccessfullyAdded_ShouldReturnSuccess()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<AddCheckInUseCase>();
        var challenge = _mock.Challenges.First(challenge => challenge.Id == MockData.PublicRunningChallengeId1_WithUserId1Active);
        var user = _mock.Users.First(user => user.Id == MockData.UserId1);
        var command = new AddCheckInCommand
        {
            ChallengeId = challenge.Id,
            UserId = user.Id,
            Date = DateTime.Today,
            Title = "Test Title",
            EvidencePhotoUrl = "Test Description"
        };

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess());
        Assert.Equal(Messages.CheckInSuccessfullyRegistered, result.GetMessage());

		var resultData = Assert.IsType<Result<CheckInModel>>(result).Data;
        Assert.Equal(challenge.Id, resultData.ChallengeId);
        Assert.Equal(user.Id, resultData.UserId);
        Assert.Equal(command.Date, resultData.Date);
        Assert.Equal(command.Title, resultData.Title);
        Assert.Equal(command.EvidencePhotoUrl, resultData.EvidencePhotoUrl);

        _mock.ChallengeRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.UserRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.ChallengeMemberRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.CheckInRepository.Verify(x => x.ListByChallengeIdAndUserIdAndDateAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>()), Times.Once);
        _mock.CheckInRepository.Verify(x => x.AddAsync(It.IsAny<CheckIn>()), Times.Once);
    }

    [Fact]
    public async Task Test_AddCheckInUseCase_ChallengeNotFound_ShouldReturnRejected()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<AddCheckInUseCase>();
        var user = _mock.Users.First(user => user.Id == MockData.UserId1);
        var command = new AddCheckInCommand
        {
            ChallengeId = Guid.NewGuid(),
            UserId = user.Id,
            Date = DateTime.Today,
            Title = "Test Title",
            EvidencePhotoUrl = "Test Description"
        };

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsRejected());
        Assert.Equal(Messages.ChallengeNotFound, result.GetMessage());

        _mock.ChallengeRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.UserRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mock.ChallengeMemberRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mock.CheckInRepository.Verify(x => x.ListByChallengeIdAndUserIdAndDateAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>()), Times.Never);
        _mock.CheckInRepository.Verify(x => x.AddAsync(It.IsAny<CheckIn>()), Times.Never);
    }

    [Fact]
    public async Task Test_AddCheckInUseCase_UserNotFound_ShouldReturnRejected()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<AddCheckInUseCase>();
        var challenge = _mock.Challenges.First(challenge => challenge.Id == MockData.PublicRunningChallengeId1_WithUserId1Active);
        var command = new AddCheckInCommand
        {
            ChallengeId = challenge.Id,
            UserId = Guid.NewGuid(),
            Date = DateTime.Today,
            Title = "Test Title",
            EvidencePhotoUrl = "Test Description"
        };

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsRejected());
        Assert.Equal(Messages.UserNotFound, result.GetMessage());

        _mock.ChallengeRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.UserRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.ChallengeMemberRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mock.CheckInRepository.Verify(x => x.ListByChallengeIdAndUserIdAndDateAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>()), Times.Never);
        _mock.CheckInRepository.Verify(x => x.AddAsync(It.IsAny<CheckIn>()), Times.Never);
    }

    [Fact]
    public async Task Test_AddCheckInUseCase_CheckInAlreadyExists_ShouldReturnRejected()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<AddCheckInUseCase>();
        var existingCheckIn = _mock.CheckIns.First(checkIn => checkIn.Id == MockData.CheckInId1);
        var command = new AddCheckInCommand
        {
            ChallengeId = existingCheckIn.ChallengeId,
            UserId = existingCheckIn.UserId,
            Date = existingCheckIn.Date,
            Title = "Test Title",
            EvidencePhotoUrl = "Test Description"
        };

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsRejected());
        Assert.Equal(Messages.UserHasReachedTheMaximumNumberOfCheckInsForTheDay, result.GetMessage());

        _mock.ChallengeRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.UserRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.ChallengeMemberRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.CheckInRepository.Verify(x => x.ListByChallengeIdAndUserIdAndDateAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>()), Times.Once);
        _mock.CheckInRepository.Verify(x => x.AddAsync(It.IsAny<CheckIn>()), Times.Never);
    }

    [Fact]
    public async Task Test_AddCheckInUseCase_Yesterday_ShouldReturnSuccess()
    {
        var useCase = _mocker.CreateInstance<AddCheckInUseCase>();
        var command = new AddCheckInCommand
        {
            ChallengeId = MockData.PublicRunningChallengeId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active,
            UserId = MockData.UserId4,
            Date = DateTime.Today.AddDays(-1),
            Title = "Test Title",
            EvidencePhotoUrl = "Test Description"
        };

        var result = await useCase.HandleAsync(command);

        Assert.True(result.IsSuccess());
        Assert.Equal(Messages.CheckInSuccessfullyRegistered, result.GetMessage());
        _mock.CheckInRepository.Verify(x => x.AddAsync(It.IsAny<CheckIn>()), Times.Once);
    }

    [Fact]
    public async Task Test_AddCheckInUseCase_DayBeforeYesterday_ShouldReturnRejected()
    {
        var useCase = _mocker.CreateInstance<AddCheckInUseCase>();
        var challenge = _mock.Challenges.First(challenge => challenge.Id == MockData.PublicRunningChallengeId1_WithUserId1Active);
        var user = _mock.Users.First(user => user.Id == MockData.UserId1);
        var command = new AddCheckInCommand
        {
            ChallengeId = challenge.Id,
            UserId = user.Id,
            Date = DateTime.Today.AddDays(-2),
            Title = "Test Title",
            EvidencePhotoUrl = "Test Description"
        };

        var result = await useCase.HandleAsync(command);

        Assert.True(result.IsRejected());
        Assert.Equal(Messages.CheckInDateMustBeTodayOrYesterday, result.GetMessage());
        _mock.CheckInRepository.Verify(x => x.AddAsync(It.IsAny<CheckIn>()), Times.Never);
    }

    [Fact]
    public async Task Test_AddCheckInUseCase_Tomorrow_ShouldReturnRejected()
    {
        var useCase = _mocker.CreateInstance<AddCheckInUseCase>();
        var challenge = _mock.Challenges.First(challenge => challenge.Id == MockData.PublicRunningChallengeId1_WithUserId1Active);
        var user = _mock.Users.First(user => user.Id == MockData.UserId1);
        var command = new AddCheckInCommand
        {
            ChallengeId = challenge.Id,
            UserId = user.Id,
            Date = DateTime.Today.AddDays(1),
            Title = "Test Title",
            EvidencePhotoUrl = "Test Description"
        };

        var result = await useCase.HandleAsync(command);

        Assert.True(result.IsRejected());
        Assert.Equal(Messages.CheckInDateMustBeTodayOrYesterday, result.GetMessage());
        _mock.CheckInRepository.Verify(x => x.AddAsync(It.IsAny<CheckIn>()), Times.Never);
    }

    [Fact]
    public async Task Test_AddCheckInUseCase_BlockedMember_ShouldReturnRejected()
    {
        var useCase = _mocker.CreateInstance<AddCheckInUseCase>();
        var command = new AddCheckInCommand
        {
            ChallengeId = MockData.PublicRunningChallengeId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active,
            UserId = MockData.UserId3,
            Date = DateTime.Today,
            Title = "Test Title",
            EvidencePhotoUrl = "Test Description"
        };

        var result = await useCase.HandleAsync(command);

        Assert.True(result.IsRejected());
        Assert.Equal(Messages.OnlyActiveMembersCanMakeCheckIns, result.GetMessage());
        _mock.CheckInRepository.Verify(x => x.AddAsync(It.IsAny<CheckIn>()), Times.Never);
    }
}
