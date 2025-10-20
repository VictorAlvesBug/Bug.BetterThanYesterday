using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.CheckIns.GetPlanWithCheckInsByPlanId;

public class GetPlanWithCheckInsByPlanIdCommand : ICommand
{
    public GetPlanWithCheckInsByPlanIdCommand(Guid planId) => PlanId = planId;
    
    public Guid PlanId { get; init; }
    
    public void Validate()
    {
        if (PlanId == Guid.Empty)
            throw new ArgumentNullException(nameof(PlanId), Messages.EnterPlanId);
    }
}