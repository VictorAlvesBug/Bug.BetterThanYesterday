namespace Bug.BetterThanYesterday.Application.DayOffs.Models;

public class DayOffModel
{
	public Guid Id { get; set; }
	public Guid PlanId { get; set; }
	public Guid UserId { get; set; }
	public DateTime Date { get; set; }
	public DateTime CreatedAt { get; set; }
}

public class UseDayOffResultModel
{
	public DayOffModel DayOff { get; set; } = null!;
	public int DaysOffAvailable { get; set; }
}
