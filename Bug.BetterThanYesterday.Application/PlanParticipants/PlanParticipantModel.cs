namespace Bug.BetterThanYesterday.Application.PlanParticipants;

public class PlanParticipantModel
{
	public string Id { get; set; }
	public string PlanId { get; set; }
	public string UserId { get; set; }
	public DateOnly JoinedAt { get; set; }
	public DateOnly? LeftAt { get; set; }
	public string Status { get; set; }
}
