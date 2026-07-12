using Bug.BetterThanYesterday.Application.CheckIns;
using Bug.BetterThanYesterday.Application.CheckIns.AddCheckIn;
using Bug.BetterThanYesterday.Application.CheckIns.Models;
using Bug.BetterThanYesterday.Application.Mocks;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.Plans;
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
        var plan = _mock.Plans.First(plan => plan.Id == MockData.PublicRunningPlanId1_WithUserId1Active);
        var user = _mock.Users.First(user => user.Id == MockData.UserId1);
        var command = new AddCheckInCommand
        {
            PlanId = plan.Id,
            UserId = user.Id,
            Date = DateTime.Today,
            Title = "Test Title",
            PhotoUrl = "Test Description"
        };

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess());
        Assert.Equal(Messages.CheckInSuccessfullyRegistered, result.GetMessage());

		var resultData = Assert.IsType<Result<CheckInModel>>(result).Data;
        Assert.Equal(plan.Id, resultData.PlanId);
        Assert.Equal(user.Id, resultData.UserId);
        Assert.Equal(command.Date, resultData.Date);
        Assert.Equal(command.Title, resultData.Title);
        Assert.Equal(command.PhotoUrl, resultData.PhotoUrl);

        _mock.PlanRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.UserRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.PlanMemberRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.CheckInRepository.Verify(x => x.ListByPlanIdAndUserIdAndDateAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>()), Times.Once);
        _mock.CheckInRepository.Verify(x => x.AddAsync(It.IsAny<CheckIn>()), Times.Once);
    }

    [Fact]
    public async Task Test_AddCheckInUseCase_PlanNotFound_ShouldReturnRejected()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<AddCheckInUseCase>();
        var user = _mock.Users.First(user => user.Id == MockData.UserId1);
        var command = new AddCheckInCommand
        {
            PlanId = Guid.NewGuid(),
            UserId = user.Id,
            Date = DateTime.Today,
            Title = "Test Title",
            PhotoUrl = "Test Description"
        };

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsRejected());
        Assert.Equal(Messages.PlanNotFound, result.GetMessage());

        _mock.PlanRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.UserRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mock.PlanMemberRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mock.CheckInRepository.Verify(x => x.ListByPlanIdAndUserIdAndDateAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>()), Times.Never);
        _mock.CheckInRepository.Verify(x => x.AddAsync(It.IsAny<CheckIn>()), Times.Never);
    }

    [Fact]
    public async Task Test_AddCheckInUseCase_UserNotFound_ShouldReturnRejected()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<AddCheckInUseCase>();
        var plan = _mock.Plans.First(plan => plan.Id == MockData.PublicRunningPlanId1_WithUserId1Active);
        var command = new AddCheckInCommand
        {
            PlanId = plan.Id,
            UserId = Guid.NewGuid(),
            Date = DateTime.Today,
            Title = "Test Title",
            PhotoUrl = "Test Description"
        };

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsRejected());
        Assert.Equal(Messages.UserNotFound, result.GetMessage());

        _mock.PlanRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.UserRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.PlanMemberRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mock.CheckInRepository.Verify(x => x.ListByPlanIdAndUserIdAndDateAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>()), Times.Never);
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
            PlanId = existingCheckIn.PlanId,
            UserId = existingCheckIn.UserId,
            Date = existingCheckIn.Date,
            Title = "Test Title",
            PhotoUrl = "Test Description"
        };

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsRejected());
        Assert.Equal(Messages.UserHasReachedTheMaximumNumberOfCheckInsForTheDay, result.GetMessage());

        _mock.PlanRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.UserRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.PlanMemberRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.CheckInRepository.Verify(x => x.ListByPlanIdAndUserIdAndDateAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>()), Times.Once);
        _mock.CheckInRepository.Verify(x => x.AddAsync(It.IsAny<CheckIn>()), Times.Never);
    }
}
