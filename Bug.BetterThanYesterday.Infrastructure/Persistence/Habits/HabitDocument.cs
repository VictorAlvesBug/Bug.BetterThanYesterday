using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Habits;

public class HabitDocument : Document
{
	public string Name { get; set; }
	public DateTime CreatedAt { get; set; }
}
