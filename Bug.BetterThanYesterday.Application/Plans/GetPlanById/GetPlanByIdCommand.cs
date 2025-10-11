using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Plans.GetPlanById;

public class GetPlanByIdCommand : ICommand
{
	public GetPlanByIdCommand(Guid planId) => PlanId = planId;

	public Guid PlanId { get; init; }

	public void Validate()
	{
		if (PlanId == Guid.Empty)
			throw new ArgumentNullException(nameof(PlanId), "Informe o ID do plano");
	}
}
