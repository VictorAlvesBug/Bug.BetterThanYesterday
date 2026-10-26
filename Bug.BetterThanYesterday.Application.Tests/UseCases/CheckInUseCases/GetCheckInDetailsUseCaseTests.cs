using Bug.BetterThanYesterday.Application.CheckIns;
using Bug.BetterThanYesterday.Application.CheckIns.GetCheckInDetails;
using Bug.BetterThanYesterday.Application.CheckIns.Models;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Strings;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.CheckInUseCases;

public class GetCheckInDetailsUseCaseTests : BaseCheckInUseCaseTests
{
    [Fact]
    public async Task Test_GetCheckInDetailsUseCase_CheckInSuccessfullyFound_ShouldReturnSuccess()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<GetCheckInDetailsUseCase>();
        var firstChallenge = _mock.Challenges[0];
        var firstUser = _mock.Users[0];
        var firstCheckIn = _mock.CheckIns.First(x => x.ChallengeId == firstChallenge.Id && x.UserId == firstUser.Id);
        var command = new GetCheckInDetailsCommand(
            firstChallenge.Id,
            firstUser.Id,
            firstCheckIn.Date,
            firstCheckIn.Index
        );

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess());
        Assert.Equal(Messages.CheckInSuccessfullyFound, result.GetMessage());

		var resultData = Assert.IsType<Result<CheckInModel>>(result).Data;
        Assert.Equal(firstCheckIn.Id, resultData.Id);
        Assert.Equal(firstCheckIn.ChallengeId, resultData.ChallengeId);
        Assert.Equal(firstCheckIn.UserId, resultData.UserId);
        Assert.Equal(firstCheckIn.Date, resultData.Date);
        Assert.Equal(firstCheckIn.Index, resultData.Index);
        Assert.Equal(firstCheckIn.Title, resultData.Title);
        Assert.Equal(firstCheckIn.PhotoUrl, resultData.EvidencePhotoUrl);

        _mock.ChallengeRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.UserRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.CheckInRepository.Verify(x => x.GetDetailsAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task Test_GetCheckInDetailsUseCase_ChallengeNotFound_ShouldReturnRejected()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<GetCheckInDetailsUseCase>();
        var firstUser = _mock.Users[0];
        var command = new GetCheckInDetailsCommand(
            Guid.NewGuid(),
            firstUser.Id,
            DateTime.Today,
            0
        );

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsRejected());
        Assert.Equal(Messages.ChallengeNotFound, result.GetMessage());

        _mock.ChallengeRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.UserRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mock.CheckInRepository.Verify(x => x.GetDetailsAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Test_GetCheckInDetailsUseCase_UserNotFound_ShouldReturnRejected()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<GetCheckInDetailsUseCase>();
        var firstChallenge = _mock.Challenges[0];
        var command = new GetCheckInDetailsCommand(
            firstChallenge.Id,
            Guid.NewGuid(),
            DateTime.Today,
            0
        );

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsRejected());
        Assert.Equal(Messages.UserNotFound, result.GetMessage());

        _mock.ChallengeRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.UserRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.CheckInRepository.Verify(x => x.GetDetailsAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Test_GetCheckInDetailsUseCase_CheckInNotFound_ShouldReturnRejected()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<GetCheckInDetailsUseCase>();
        var firstChallenge = _mock.Challenges[0];
        var firstUser = _mock.Users[0];
        var command = new GetCheckInDetailsCommand(
            firstChallenge.Id,
            firstUser.Id,
            DateTime.Today.AddYears(1),
            999
        );

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsRejected());
        Assert.Equal(Messages.CheckInNotFound, result.GetMessage());

        _mock.ChallengeRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.UserRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.CheckInRepository.Verify(x => x.GetDetailsAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<int>()), Times.Once);
    }
}