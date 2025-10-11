namespace Bug.BetterThanYesterday.Application.Plans;

public class PlanModel
{
	public Guid PlanId { get; set; }
	public Guid HabitId { get; set; }
	public string? Description { get; set; }
	public DateTime StartsAt { get; set; }
	public DateTime EndsAt { get; set; }
	public int StatusId { get; set; }
	public string StatusName { get; set; }
	public int TypeId { get; set; }
	public string TypeName { get; set; }
	public DateTime CreatedAt { get; set; }
}
