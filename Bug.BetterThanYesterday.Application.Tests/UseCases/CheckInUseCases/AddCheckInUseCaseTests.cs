using Bug.BetterThanYesterday.Application.CheckIns;
using Bug.BetterThanYesterday.Application.CheckIns.AddCheckIn;
using Bug.BetterThanYesterday.Application.CheckIns.Models;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.Strings;
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
        var plan = _mock.Plans.First(plan => plan.Id == PlanRepositoryMockFactory.PublicRunningPlanId1_WithUserId1Active);
        var user = _mock.Users.First(user => user.Id == UserRepositoryMockFactory.UserId1);
        var command = new AddCheckInCommand(
            plan.Id,
            user.Id,
            DateTime.Today,
            "Test Title",
            "Test Description"
        );

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
        Assert.Equal(command.Description, resultData.Description);

 _mock.CheckInRepository            .Verify(x => x.AddAsync(It.Is<CheckIn>(c =>
                c.PlanId == plan.Id &&
                c.UserId == user.Id &&
                c.Date == DateOnly.FromDateTime(command.Date) &&
                c.Title == command.Title &&
                c.Description == command.Description)), Times.Once);
    }

    [Fact]
    public async Task Test_AddCheckInUseCase_PlanNotFound_ShouldReturnRejected()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<AddCheckInUseCase>();
        var user = _mock.Users.First(user => user.Id == UserRepositoryMockFactory.UserId1);
        var command = new AddCheckInCommand(
            Guid.NewGuid(),
            user.Id,
            DateTime.Today,
            "Test Title",
            "Test Description"
        );

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsRejected());
        Assert.Equal(Messages.PlanNotFound, result.GetMessage());
    }

    [Fact]
    public async Task Test_AddCheckInUseCase_UserNotFound_ShouldReturnRejected()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<AddCheckInUseCase>();
        var plan = _mock.Plans.First(plan => plan.Id == PlanRepositoryMockFactory.PublicRunningPlanId1_WithUserId1Active);
        var command = new AddCheckInCommand(
            plan.Id,
            Guid.NewGuid(),
            DateTime.Today,
            "Test Title",
            "Test Description"
        );

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsRejected());
        Assert.Equal(Messages.UserNotFound, result.GetMessage());
    }

    [Fact]
    public async Task Test_AddCheckInUseCase_CheckInAlreadyExists_ShouldReturnRejected()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<AddCheckInUseCase>();
        var existingCheckIn = _mock.CheckIns.First(checkIn => checkIn.Id == CheckInRepositoryMockFactory.CheckInId1);
        var command = new AddCheckInCommand(
            existingCheckIn.PlanId,
            existingCheckIn.UserId,
            existingCheckIn.Date.ToDateTime(TimeOnly.MinValue),
            //existingCheckIn.Index,
            "Test Title",
            "Test Description"
        );

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsRejected());
        Assert.Equal(Messages.UserHasReachedTheMaximumNumberOfCheckInsForTheDay, result.GetMessage());
    }
}
