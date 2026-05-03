using Bug.BetterThanYesterday.Application.Habits;

namespace Bug.BetterThanYesterday.Application.Plans;

public class PlanModel
{
	public Guid PlanId { get; set; }
	public Guid OwnerId { get; set; }
	public Guid HabitId { get; set; }
	public string HabitName { get; set; }
	public string? Description { get; set; }
	public DateTime StartsAt { get; set; }
	public DateTime EndsAt { get; set; }
	public string Status { get; set; }
	public string Type { get; set; }
	public int DaysOffPerWeek { get; set; }
	public decimal PenaltyValue { get; set; }
	public DateTime CreatedAt { get; set; }
}
