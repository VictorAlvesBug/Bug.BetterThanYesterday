using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Plans.CancelPlan;

public class CancelPlanCommand : ICommand
{
	public CancelPlanCommand(Guid id) => Id = id;

	public Guid Id { get; init; }

	public void Validate()
	{
		if (Id == Guid.Empty)
			throw new ArgumentNullException(nameof(Id), "Informe o ID do plano");
	}
}
