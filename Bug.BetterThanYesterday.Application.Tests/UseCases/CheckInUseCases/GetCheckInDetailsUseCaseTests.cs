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
        var firstPlan = _mock.Plans[0];
        var firstUser = _mock.Users[0];
        var firstCheckIn = _mock.CheckIns.First(x => x.PlanId == firstPlan.Id && x.UserId == firstUser.Id);
        var command = new GetCheckInDetailsCommand(
            firstPlan.Id,
            firstUser.Id,
            firstCheckIn.Date.ToDateTime(TimeOnly.MinValue),
            firstCheckIn.Index
        );

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess());
        Assert.Equal(Messages.CheckInSuccessfullyFound, result.GetMessage());

		var resultData = Assert.IsType<Result<CheckInModel>>(result).Data;
        Assert.Equal(firstCheckIn.Id, resultData.CheckInId);
        Assert.Equal(firstCheckIn.PlanId, resultData.PlanId);
        Assert.Equal(firstCheckIn.UserId, resultData.UserId);
        Assert.Equal(firstCheckIn.Date.ToDateTime(TimeOnly.MinValue), resultData.Date);
        Assert.Equal(firstCheckIn.Index, resultData.Index);
        Assert.Equal(firstCheckIn.Title, resultData.Title);
        Assert.Equal(firstCheckIn.Description, resultData.Description);

        _mock.PlanRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.UserRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.CheckInRepository.Verify(x => x.GetDetailsAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task Test_GetCheckInDetailsUseCase_PlanNotFound_ShouldReturnRejected()
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
        Assert.Equal(Messages.PlanNotFound, result.GetMessage());

        _mock.PlanRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.UserRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mock.CheckInRepository.Verify(x => x.GetDetailsAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Test_GetCheckInDetailsUseCase_UserNotFound_ShouldReturnRejected()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<GetCheckInDetailsUseCase>();
        var firstPlan = _mock.Plans[0];
        var command = new GetCheckInDetailsCommand(
            firstPlan.Id,
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

        _mock.PlanRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.UserRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.CheckInRepository.Verify(x => x.GetDetailsAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Test_GetCheckInDetailsUseCase_CheckInNotFound_ShouldReturnRejected()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<GetCheckInDetailsUseCase>();
        var firstPlan = _mock.Plans[0];
        var firstUser = _mock.Users[0];
        var command = new GetCheckInDetailsCommand(
            firstPlan.Id,
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

        _mock.PlanRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.UserRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.CheckInRepository.Verify(x => x.GetDetailsAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<int>()), Times.Once);
    }
}