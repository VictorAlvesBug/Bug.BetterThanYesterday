using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.CheckIns
{
	public class CheckInDocument : Document
	{
		public string PlanId { get; set; }
		public string UserId { get; set; }
		public DateOnly Date { get; set; }
		public string Title { get; set; }
		public string? Description { get; set; }
	}
}
