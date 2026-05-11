using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Plans.ListPlansByFilter;

public class ListPlansByFilterCommand : ICommand
{
	public Guid? OwnerId { get; set; }
	public Guid? HabitId { get; set; }
	public string? Status { get; set; }
	public string? Type { get; set; }

	public void Validate() { }
}
