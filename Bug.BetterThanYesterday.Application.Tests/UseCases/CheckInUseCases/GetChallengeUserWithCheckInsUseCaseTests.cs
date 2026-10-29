using Bug.BetterThanYesterday.Application.CheckIns;
using Bug.BetterThanYesterday.Application.CheckIns.GetChallengeUserWithCheckIns;
using Bug.BetterThanYesterday.Application.CheckIns.Models;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.CheckInUseCases;

public class GetChallengeUserWithCheckInsUseCaseTests : BaseCheckInUseCaseTests
{
    [Fact]
    public async Task Test_GetChallengeUserWithCheckInsUseCase_ChallengeUserSuccessfullyFound_ShouldReturnSuccess()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<GetChallengeUserWithCheckInsUseCase>();
        var firstChallenge = _mock.Challenges[0];
        var firstUser = _mock.Users[0];
        var checkIns = _mock.CheckIns.Where(x =>
            x.ChallengeId == firstChallenge.Id &&
            x.UserId == firstUser.Id).ToList();
        var command = new GetChallengeUserWithCheckInsCommand(firstChallenge.Id, firstUser.Id);

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess());
        Assert.Equal(Messages.CheckInSuccessfullyFound, result.GetMessage());

        var resultData = Assert.IsType<Result<ChallengeUserWithCheckInsModel>>(result).Data;
        Assert.Equal(checkIns.Count, resultData.CheckIns.Count);
		
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

        var userCheckIns = _mock.CheckIns.Where(x => x.ChallengeId == firstChallenge.Id && x.UserId == firstUser.Id).ToList();
        Assert.Equal(userCheckIns.Count, resultData.CheckIns.Count);
        
        foreach (var checkIn in userCheckIns)
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
        _mock.CheckInRepository.Verify(x => x.ListByChallengeIdAndUserIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Test_GetChallengeUserWithCheckInsUseCase_ChallengeNotFound_ShouldReturnRejected()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<GetChallengeUserWithCheckInsUseCase>();
        var firstUser = _mock.Users[0];
        var command = new GetChallengeUserWithCheckInsCommand(Guid.NewGuid(), firstUser.Id);

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsRejected());
        Assert.Equal(Messages.ChallengeNotFound, result.GetMessage());

        _mock.ChallengeRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.UserRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mock.CheckInRepository.Verify(x => x.ListByChallengeIdAndUserIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task Test_GetChallengeUserWithCheckInsUseCase_UserNotFound_ShouldReturnRejected()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<GetChallengeUserWithCheckInsUseCase>();
        var firstChallenge = _mock.Challenges[0];
        var command = new GetChallengeUserWithCheckInsCommand(firstChallenge.Id, Guid.NewGuid());

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsRejected());
        Assert.Equal(Messages.UserNotFound, result.GetMessage());

        _mock.ChallengeRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.UserRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.CheckInRepository.Verify(x => x.ListByChallengeIdAndUserIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
    }
}