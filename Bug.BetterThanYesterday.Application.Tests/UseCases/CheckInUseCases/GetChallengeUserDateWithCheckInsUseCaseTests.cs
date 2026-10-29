using Bug.BetterThanYesterday.Application.CheckIns;
using Bug.BetterThanYesterday.Application.CheckIns.GetChallengeUserDateWithCheckIns;
using Bug.BetterThanYesterday.Application.CheckIns.Models;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.CheckInUseCases;

public class GetChallengeUserDateWithCheckInsUseCaseTests : BaseCheckInUseCaseTests
{
    [Fact]
    public async Task Test_GetChallengeUserDateWithCheckInsUseCase_ChallengeUserDateSuccessfullyFound_ShouldReturnSuccess()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<GetChallengeUserDateWithCheckInsUseCase>();
        var firstChallenge = _mock.Challenges[0];
        var firstUser = _mock.Users[0];
        var checkInDate = _mock.CheckIns.First(x => x.ChallengeId == firstChallenge.Id && x.UserId == firstUser.Id).Date;
        var checkIns = _mock.CheckIns.Where(x =>
            x.ChallengeId == firstChallenge.Id &&
            x.UserId == firstUser.Id &&
            DateOnly.FromDateTime(x.Date) == DateOnly.FromDateTime(checkInDate)).ToList();
        var command = new GetChallengeUserDateWithCheckInsCommand(
            firstChallenge.Id,
            firstUser.Id,
            checkInDate
        );

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess());
        Assert.Equal(Messages.CheckInsSuccessfullyFound, result.GetMessage());

        var resultData = Assert.IsType<Result<ChallengeUserDateWithCheckInsModel>>(result).Data;
        Assert.Equal(checkIns.Count, resultData.CheckIns.Count);
        
		Assert.Equal(checkInDate, resultData.Date);
		
		Assert.Equal(firstChallenge.Id, resultData.Challenge.Id);
		Assert.Equal(firstChallenge.OwnerId, resultData.Challenge.OwnerId);
		Assert.Equal(firstChallenge.HabitId, resultData.Challenge.HabitId);
		Assert.Equal(firstChallenge.Description, resultData.Challenge.Description);
		Assert.Equal(firstChallenge.StartsAt.ToDateTime(TimeOnly.MinValue), resultData.Challenge.StartsAt);
		Assert.Equal(firstChallenge.EndsAt.ToDateTime(TimeOnly.MinValue), resultData.Challenge.EndsAt);
		Assert.Equal(firstChallenge.GetStatus(), ChallengeStatus.Get(resultData.Challenge.Status));
		Assert.Equal(firstChallenge.Type, ChallengeType.Get(resultData.Challenge.Type));
		Assert.Equal(firstChallenge.DaysOffPerWeek, resultData.Challenge.DaysOffPerWeek);
		Assert.Equal(firstChallenge.PenaltyValue, resultData.Challenge.PenaltyValue);
		Assert.Equal(firstChallenge.CreatedAt, resultData.Challenge.CreatedAt);
		
		Assert.Equal(firstUser.Id, resultData.User.Id);
		Assert.Equal(firstUser.Name, resultData.User.Name);
		Assert.Equal(firstUser.Email.Value, resultData.User.Email);
		Assert.Equal(firstUser.CreatedAt, resultData.User.CreatedAt);

        var dateCheckIns = _mock.CheckIns.Where(x =>
            x.ChallengeId == firstChallenge.Id &&
            x.UserId == firstUser.Id &&
            DateOnly.FromDateTime(x.Date) == DateOnly.FromDateTime(checkInDate)).ToList();

        Assert.Equal(dateCheckIns.Count, resultData.CheckIns.Count);
        
        foreach (var checkIn in dateCheckIns)
        {
            var resultCheckIn = resultData.CheckIns.FirstOrDefault(x => 
                x.Id == checkIn.Id &&
                x.ChallengeId == checkIn.ChallengeId &&
                x.UserId == checkIn.UserId &&
                x.Date == checkIn.Date &&
                x.Index == checkIn.Index &&
                x.Title == checkIn.Title &&
                x.EvidencePhotoUrl == checkIn.PhotoUrl);

            Assert.NotNull(resultCheckIn);
        }

        _mock.ChallengeRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.UserRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.CheckInRepository.Verify(x => x.ListByChallengeIdAndUserIdAndDateAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>()), Times.Once);
    }

    [Fact]
    public async Task Test_GetChallengeUserDateWithCheckInsUseCase_ChallengeNotFound_ShouldReturnRejected()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<GetChallengeUserDateWithCheckInsUseCase>();
        var firstUser = _mock.Users[0];
        var command = new GetChallengeUserDateWithCheckInsCommand(
            Guid.NewGuid(),
            firstUser.Id,
            DateTime.Today
        );

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsRejected());
        Assert.Equal(Messages.ChallengeNotFound, result.GetMessage());

        _mock.ChallengeRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.UserRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mock.CheckInRepository.Verify(x => x.ListByChallengeIdAndUserIdAndDateAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>()), Times.Never);
    }

    [Fact]
    public async Task Test_GetChallengeUserDateWithCheckInsUseCase_UserNotFound_ShouldReturnRejected()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<GetChallengeUserDateWithCheckInsUseCase>();
        var firstChallenge = _mock.Challenges[0];
        var command = new GetChallengeUserDateWithCheckInsCommand(
            firstChallenge.Id,
            Guid.NewGuid(),
            DateTime.Today
        );

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsRejected());
        Assert.Equal(Messages.UserNotFound, result.GetMessage());

        _mock.ChallengeRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.UserRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.CheckInRepository.Verify(x => x.ListByChallengeIdAndUserIdAndDateAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>()), Times.Never);
    }

    [Fact]
    public async Task Test_GetChallengeUserDateWithCheckInsUseCase_NoCheckInsFound_ShouldReturnSuccess()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<GetChallengeUserDateWithCheckInsUseCase>();
        var firstChallenge = _mock.Challenges[0];
        var firstUser = _mock.Users[0];
        var command = new GetChallengeUserDateWithCheckInsCommand(
            firstChallenge.Id,
            firstUser.Id,
            DateTime.Today.AddYears(1)
        );

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess());
        Assert.Equal(Messages.CheckInNotFound, result.GetMessage());

        var resultData = Assert.IsType<Result<ChallengeUserDateWithCheckInsModel>>(result).Data;
        Assert.Empty(resultData.CheckIns);

        _mock.ChallengeRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.UserRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.CheckInRepository.Verify(x => x.ListByChallengeIdAndUserIdAndDateAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>()), Times.Once);
    }
}