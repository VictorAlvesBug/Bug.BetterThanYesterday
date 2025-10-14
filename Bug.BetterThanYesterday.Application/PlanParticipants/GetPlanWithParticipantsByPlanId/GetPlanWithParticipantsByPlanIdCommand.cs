using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.PlanParticipants.GetPlanWithParticipantsByPlanId;

public class GetPlanWithParticipantsByPlanIdCommand : ICommand
{
    public GetPlanWithParticipantsByPlanIdCommand(Guid planId)
    {
        PlanId = planId;
    }

    public Guid PlanId { get; init; }

    public void Validate()
    {
        if (PlanId == Guid.Empty)
            throw new ArgumentNullException(nameof(PlanId), "Informe o ID do plano");
    }
}