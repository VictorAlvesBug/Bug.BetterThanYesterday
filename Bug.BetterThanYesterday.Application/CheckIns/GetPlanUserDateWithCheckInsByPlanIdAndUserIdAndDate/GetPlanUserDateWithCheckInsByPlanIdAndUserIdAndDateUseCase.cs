using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;

namespace Bug.BetterThanYesterday.Application.CheckIns.GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDate;

public class GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDateUseCase(
    ICheckInRepository checkInRepository)
    : IUseCase<GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDateCommand>
{
    public Task<IResult> HandleAsync(GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDateCommand command)
    {
        throw new NotImplementedException();
    }

}