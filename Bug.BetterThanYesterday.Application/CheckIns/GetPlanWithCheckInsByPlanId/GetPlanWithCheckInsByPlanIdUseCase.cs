using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;

namespace Bug.BetterThanYesterday.Application.CheckIns.GetPlanWithCheckInsByPlanId;

public class GetPlanWithCheckInsByPlanIdUseCase(
    ICheckInRepository checkInRepository)
    : IUseCase<GetPlanWithCheckInsByPlanIdCommand>
{
    public Task<IResult> HandleAsync(GetPlanWithCheckInsByPlanIdCommand command)
    {
        throw new NotImplementedException();
    }

}