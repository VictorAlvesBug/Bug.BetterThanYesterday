using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Plans.UpdatePlanStatus;

public class UpdatePlanStatusCommand : ICommand
{
	public UpdatePlanStatusCommand(Guid id, int statusId)
	{
		Id = id;
		StatusId = statusId;
	}

	public Guid Id { get; init; }
	public int StatusId { get; init; }

	public void Validate()
	{
		if (Id == Guid.Empty)
			throw new ArgumentNullException(nameof(Id), "Informe o ID do plano");

		if (StatusId <= 0)
			throw new ArgumentException("Informe o status do plano", nameof(StatusId));
	}
}
