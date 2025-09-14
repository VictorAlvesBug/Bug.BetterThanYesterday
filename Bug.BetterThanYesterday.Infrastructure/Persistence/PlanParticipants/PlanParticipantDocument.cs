using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.PlanParticipants;

public class PlanParticipantDocument : Document
{
	public string PlanId { get; set; }
	public string UserId { get; set; }
	public DateOnly JoinedAt { get; set; }
	public DateOnly? LeftAt { get; set; }
	public string Status { get; set; }
}
