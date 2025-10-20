using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.PlanParticipants.RemoveUserFromPlan;

public class RemoveUserFromPlanCommand : ICommand
{
    public RemoveUserFromPlanCommand(Guid planId, Guid userId)
    {
        PlanId = planId;
        UserId = userId;
    }

    public Guid PlanId { get; init; }
    public Guid UserId { get; init; }

    public void Validate()
    {
        if (PlanId == Guid.Empty)
            throw new ArgumentNullException(nameof(PlanId), Messages.EnterPlanId);
        
        if (UserId == Guid.Empty)
            throw new ArgumentNullException(nameof(UserId), Messages.EnterUserId);
    }
}