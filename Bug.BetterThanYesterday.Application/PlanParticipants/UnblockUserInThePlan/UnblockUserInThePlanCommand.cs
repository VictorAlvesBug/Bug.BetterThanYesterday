using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.PlanParticipants.UnblockUserInThePlan;

public class UnblockUserInThePlanCommand : ICommand
{
    public UnblockUserInThePlanCommand(Guid planId, Guid userId)
    {
        PlanId = planId;
        UserId = userId;
    }

    public Guid PlanId { get; init; }
    public Guid UserId { get; init; }

    public void Validate()
    {
        if (PlanId == Guid.Empty)
            throw new ArgumentNullException(nameof(PlanId), "Informe o ID do plano");
        
        if (UserId == Guid.Empty)
            throw new ArgumentNullException(nameof(UserId), "Informe o ID do usuário");
    }
}