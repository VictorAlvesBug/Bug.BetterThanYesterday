using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Plans;

public class PlanDocument : Document
{
	public string HabitId { get; set; }
	public string? Description { get; set; }
	public DateTime StartsAt { get; set; }
	public DateTime EndsAt { get; set; }
	public int Status { get; set; }
	public int Type { get; set; }
	public DateTime CreatedAt { get; set; }
}
