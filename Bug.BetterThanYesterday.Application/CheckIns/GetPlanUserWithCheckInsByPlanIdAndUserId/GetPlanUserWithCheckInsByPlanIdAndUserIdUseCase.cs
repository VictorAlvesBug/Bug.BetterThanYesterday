using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;

namespace Bug.BetterThanYesterday.Application.CheckIns.GetPlanUserWithCheckInsByPlanIdAndUserId;

public class GetPlanUserWithCheckInsByPlanIdAndUserIdUseCase(
    ICheckInRepository checkInRepository)
    : IUseCase<GetPlanUserWithCheckInsByPlanIdAndUserIdCommand>
{
    public Task<IResult> HandleAsync(GetPlanUserWithCheckInsByPlanIdAndUserIdCommand command)
    {
        throw new NotImplementedException();
    }

}