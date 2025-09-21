namespace Bug.BetterThanYesterday.Application.Plans;

public class PlanModel
{
	public string Id { get; set; }
	public string HabitId { get; set; }
	public string? Description { get; set; }
	public DateOnly StartsAt { get; set; }
	public DateOnly EndsAt { get; set; }
	public int Status { get; set; }
	public int Type { get; set; }
	public DateOnly CreatedAt { get; set; }
}
