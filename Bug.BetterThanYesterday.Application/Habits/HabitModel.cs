using Bug.BetterThanYesterday.Application.Plans;

namespace Bug.BetterThanYesterday.Application.Habits;

public class HabitModel
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public DateTime CreatedAt { get; set; }
	public List<PlanModel> Plans { get; set; }

}
