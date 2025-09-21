namespace Bug.BetterThanYesterday.Application.PlanParticipants;

public class PlanParticipantModel
{
	public Guid Id { get; set; }
	public Guid PlanId { get; set; }
	public Guid UserId { get; set; }
	public DateTime JoinedAt { get; set; }
	public DateTime? LeftAt { get; set; }
	public int StatusId { get; set; }
	public string StatusName { get; set; }
}
