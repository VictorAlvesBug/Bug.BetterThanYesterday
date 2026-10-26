using Bug.BetterThanYesterday.Application.CheckIns;
using Bug.BetterThanYesterday.Application.CheckIns.GetChallengeWithCheckInsByChallengeId;
using Bug.BetterThanYesterday.Application.CheckIns.Models;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.CheckInUseCases;

public class GetChallengeWithCheckInsByChallengeIdUseCaseTests : BaseCheckInUseCaseTests
{
    [Fact]
    public async Task Test_GetChallengeWithCheckInsByChallengeIdUseCase_ChallengeSuccessfullyFound_ShouldReturnSuccess()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<GetChallengeWithCheckInsByChallengeIdUseCase>();
        var firstChallenge = _mock.Challenges[0];
        var owner = _mock.Users.First(user => user.Id == firstChallenge.OwnerId);
        var checkIns = _mock.CheckIns.Where(x => x.ChallengeId == firstChallenge.Id).ToList();
        var command = new GetChallengeWithCheckInsByChallengeIdCommand(firstChallenge.Id);

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess());
        Assert.Equal(Messages.CheckInSuccessfullyFound, result.GetMessage());

        var resultData = Assert.IsType<Result<ChallengeWithCheckInsModel>>(result).Data;
        Assert.Equal(checkIns.Count, resultData.CheckIns.Count);
		
		Assert.Equal(firstChallenge.Id, resultData.Challenge.Id);
		Assert.Equal(firstChallenge.OwnerId, resultData.Challenge.OwnerId);
		Assert.Equal(owner.Nickname, resultData.Challenge.OwnerName);
		Assert.Equal(firstChallenge.HabitId, resultData.Challenge.HabitId);
		Assert.Equal(firstChallenge.Description, resultData.Challenge.Description);
		Assert.Equal(firstChallenge.StartsAt.ToDateTime(TimeOnly.MinValue), resultData.Challenge.StartsAt);
		Assert.Equal(firstChallenge.EndsAt.ToDateTime(TimeOnly.MinValue), resultData.Challenge.EndsAt);
		Assert.Equal(firstChallenge.GetStatus(), ChallengeStatus.Get(resultData.Challenge.Status));
		Assert.Equal(firstChallenge.Type, ChallengeType.Get(resultData.Challenge.Type));
		Assert.Equal(firstChallenge.DaysOffPerWeek, resultData.Challenge.DaysOffPerWeek);
		Assert.Equal(firstChallenge.PenaltyValue, resultData.Challenge.PenaltyValue);
		Assert.Equal(firstChallenge.CreatedAt, resultData.Challenge.CreatedAt);

        var challengeCheckIns = _mock.CheckIns.Where(x => x.ChallengeId == firstChallenge.Id).ToList();
        Assert.Equal(challengeCheckIns.Count, resultData.CheckIns.Count);
        
        foreach (var checkIn in challengeCheckIns)
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
        _mock.CheckInRepository.Verify(x => x.ListByChallengeIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Test_GetChallengeWithCheckInsByChallengeIdUseCase_ChallengeNotFound_ShouldReturnRejected()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<GetChallengeWithCheckInsByChallengeIdUseCase>();
        var command = new GetChallengeWithCheckInsByChallengeIdCommand(Guid.NewGuid());

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsRejected());
        Assert.Equal(Messages.ChallengeNotFound, result.GetMessage());

        _mock.ChallengeRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.CheckInRepository.Verify(x => x.ListByChallengeIdAsync(It.IsAny<Guid>()), Times.Never);
    }
}