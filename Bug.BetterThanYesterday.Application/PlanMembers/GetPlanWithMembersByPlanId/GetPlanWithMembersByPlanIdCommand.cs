using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.PlanMembers.GetPlanWithMembersByPlanId;

public class GetPlanWithMembersByPlanIdCommand : ICommand
{
    public GetPlanWithMembersByPlanIdCommand(Guid planId)
    {
        PlanId = planId;
    }

    public Guid PlanId { get; init; }

    public void Validate()
    {
        if (PlanId == Guid.Empty)
            throw new ArgumentNullException(nameof(PlanId), Messages.EnterPlanId);
    }
}