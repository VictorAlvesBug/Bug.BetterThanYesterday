using Bug.BetterThanYesterday.Application.CheckIns;
using Bug.BetterThanYesterday.Application.CheckIns.GetPlanWithCheckInsByPlanId;
using Bug.BetterThanYesterday.Application.CheckIns.Models;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Strings;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.CheckInUseCases;

public class GetPlanWithCheckInsByPlanIdUseCaseTests : BaseCheckInUseCaseTests
{
    [Fact]
    public async Task Test_GetPlanWithCheckInsByPlanIdUseCase_PlanSuccessfullyFound_ShouldReturnSuccess()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<GetPlanWithCheckInsByPlanIdUseCase>();
        var firstPlan = _mock.Plans[0];
        var checkIns = _mock.CheckIns.Where(x => x.PlanId == firstPlan.Id).ToList();
        var command = new GetPlanWithCheckInsByPlanIdCommand(firstPlan.Id);

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess());
        Assert.Equal(Messages.CheckInSuccessfullyFound, result.GetMessage());

        var resultData = Assert.IsType<Result<PlanWithCheckInsModel>>(result).Data;
        Assert.Equal(checkIns.Count, resultData.CheckIns.Count);
		
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

        var planCheckIns = _mock.CheckIns.Where(x => x.PlanId == firstPlan.Id).ToList();
        Assert.Equal(planCheckIns.Count, resultData.CheckIns.Count);
        
        foreach (var checkIn in planCheckIns)
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
    }

    [Fact]
    public async Task Test_GetPlanWithCheckInsByPlanIdUseCase_PlanNotFound_ShouldReturnRejected()
    {
        // Arrange
        var useCase = _mocker.CreateInstance<GetPlanWithCheckInsByPlanIdUseCase>();
        var command = new GetPlanWithCheckInsByPlanIdCommand(Guid.NewGuid());

        // Act
        var result = await useCase.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsRejected());
        Assert.Equal(Messages.PlanNotFound, result.GetMessage());
    }
}