using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Plans
{
	public class PlanDocument : Document
	{
		public string HabitId { get; set; }
		public string? Description { get; set; }
		public DateOnly StartsAt { get; set; }
		public DateOnly EndsAt { get; set; }
		public string Status { get; set; }
		public string Type { get; set; }
		public DateOnly CreatedAt { get; set; }
	}
}
