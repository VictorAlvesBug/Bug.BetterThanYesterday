using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Plans.UpdatePlanStatus;

public class UpdatePlanStatusCommand : ICommand
{
	public UpdatePlanStatusCommand(string id, int status)
	{
		Id = id;
		Status = status;
	}

	public string Id { get; init; }
	public int Status { get; init; }

	public void Validate()
	{
		if (string.IsNullOrWhiteSpace(Id))
			throw new ArgumentNullException(nameof(Id), "Informe o ID do plano");

		if (Status <= 0)
			throw new ArgumentException("Informe o status do plano", nameof(Status));
	}
}
