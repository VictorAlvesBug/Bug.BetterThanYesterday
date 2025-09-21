using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Plans.CancelPlan;

public class CancelPlanCommand : ICommand
{
	public CancelPlanCommand(string id) => Id = id;

	public string Id { get; }

	public void Validate()
	{
		if (string.IsNullOrWhiteSpace(Id))
			throw new ArgumentNullException(nameof(Id), "Informe o ID do plano");
	}
}
