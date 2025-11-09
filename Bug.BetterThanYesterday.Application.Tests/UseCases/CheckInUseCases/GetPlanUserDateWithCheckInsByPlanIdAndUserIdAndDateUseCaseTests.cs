using Bug.BetterThanYesterday.Application.CheckIns;
using Bug.BetterThanYesterday.Application.CheckIns.GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDate;
using Bug.BetterThanYesterday.Application.CheckIns.Models;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Strings;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.CheckInUseCases;

public class GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDateUseCaseTests : BaseCheckInUseCaseTests
{
    [Fact]
    public async Task Test_GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDateUseCase_PlanUserDateSuccessfullyFound_ShouldReturnSuccess()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDateUseCase>();
        var firstPlan = _mock.Plans[0];
        var firstUser = _mock.Users[0];
        var checkInDate = _mock.CheckIns.First(x => x.PlanId == firstPlan.Id && x.UserId == firstUser.Id).Date;
        var checkIns = _mock.CheckIns.Where(x =>
            x.PlanId == firstPlan.Id &&
            x.UserId == firstUser.Id &&
            x.Date == checkInDate).ToList();
        var command = new GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDateCommand(
            firstPlan.Id,
            firstUser.Id,
            checkInDate.ToDateTime(TimeOnly.MinValue)
        );

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess());
        Assert.Equal(Messages.CheckInsSuccessfullyFound, result.GetMessage());

        var resultData = Assert.IsType<Result<PlanUserDateWithCheckInsModel>>(result).Data;
        Assert.Equal(checkIns.Count, resultData.CheckIns.Count);
        
		Assert.Equal(checkInDate.ToDateTime(TimeOnly.MinValue), resultData.Date);
		
		Assert.Equal(firstPlan.Id, resultData.Plan.PlanId);
		Assert.Equal(firstPlan.HabitId, resultData.Plan.HabitId);
		Assert.Equal(firstPlan.Description, resultData.Plan.Description);
		Assert.Equal(firstPlan.StartsAt.ToDateTime(TimeOnly.MinValue), resultData.Plan.StartsAt);
		Assert.Equal(firstPlan.EndsAt.ToDateTime(TimeOnly.MinValue), resultData.Plan.EndsAt);
		Assert.Equal(firstPlan.Status.Id, resultData.Plan.StatusId);
		Assert.Equal(firstPlan.Status.Name, resultData.Plan.StatusName);
		Assert.Equal(firstPlan.Type.Id, resultData.Plan.TypeId);
		Assert.Equal(firstPlan.Type.Name, resultData.Plan.TypeName);
		Assert.Equal(firstPlan.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.Plan.CreatedAt);
		
		Assert.Equal(firstUser.Id, resultData.User.UserId);
		Assert.Equal(firstUser.Name, resultData.User.Name);
		Assert.Equal(firstUser.Email.Value, resultData.User.Email);
		Assert.Equal(firstUser.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.User.CreatedAt);

        var dateCheckIns = _mock.CheckIns.Where(x =>
            x.PlanId == firstPlan.Id &&
            x.UserId == firstUser.Id &&
            x.Date == checkInDate).ToList();

        Assert.Equal(dateCheckIns.Count, resultData.CheckIns.Count);
        
        foreach (var checkIn in dateCheckIns)
        {
            var resultCheckIn = resultData.CheckIns.FirstOrDefault(x => 
                x.CheckInId == checkIn.Id &&
                x.PlanId == checkIn.PlanId &&
                x.UserId == checkIn.UserId &&
                x.Date == checkIn.Date.ToDateTime(TimeOnly.MinValue) &&
                x.Index == checkIn.Index &&
                x.Title == checkIn.Title &&
                x.Description == checkIn.Description);

            Assert.NotNull(resultCheckIn);
        }

        _mock.PlanRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.UserRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.CheckInRepository.Verify(x => x.ListByPlanIdAndUserIdAndDateAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>()), Times.Once);
    }

    [Fact]
    public async Task Test_GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDateUseCase_PlanNotFound_ShouldReturnRejected()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDateUseCase>();
        var firstUser = _mock.Users[0];
        var command = new GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDateCommand(
            Guid.NewGuid(),
            firstUser.Id,
            DateTime.Today
        );

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsRejected());
        Assert.Equal(Messages.PlanNotFound, result.GetMessage());

        _mock.PlanRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.UserRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mock.CheckInRepository.Verify(x => x.ListByPlanIdAndUserIdAndDateAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>()), Times.Never);
    }

    [Fact]
    public async Task Test_GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDateUseCase_UserNotFound_ShouldReturnRejected()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDateUseCase>();
        var firstPlan = _mock.Plans[0];
        var command = new GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDateCommand(
            firstPlan.Id,
            Guid.NewGuid(),
            DateTime.Today
        );

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsRejected());
        Assert.Equal(Messages.UserNotFound, result.GetMessage());

        _mock.PlanRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.UserRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.CheckInRepository.Verify(x => x.ListByPlanIdAndUserIdAndDateAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>()), Times.Never);
    }

    [Fact]
    public async Task Test_GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDateUseCase_NoCheckInsFound_ShouldReturnSuccess()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDateUseCase>();
        var firstPlan = _mock.Plans[0];
        var firstUser = _mock.Users[0];
        var command = new GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDateCommand(
            firstPlan.Id,
            firstUser.Id,
            DateTime.Today.AddYears(1)
        );

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess());
        Assert.Equal(Messages.CheckInNotFound, result.GetMessage());

        var resultData = Assert.IsType<Result<PlanUserDateWithCheckInsModel>>(result).Data;
        Assert.Empty(resultData.CheckIns);

        _mock.PlanRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.UserRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mock.CheckInRepository.Verify(x => x.ListByPlanIdAndUserIdAndDateAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>()), Times.Once);
    }
}