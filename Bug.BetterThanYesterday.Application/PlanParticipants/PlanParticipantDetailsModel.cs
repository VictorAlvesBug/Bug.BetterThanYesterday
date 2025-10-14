using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Application.Users;

namespace Bug.BetterThanYesterday.Application.PlanParticipants;

public class PlanParticipantDetailsModel
{
	public Guid PlanParticipantId { get; set; }
	public DateTime JoinedAt { get; set; }
	public DateTime? LeftAt { get; set; }
	public int StatusId { get; set; }
	public string StatusName { get; set; }
	public PlanModel Plan { get; set; }
	public UserModel Participant { get; set; }
}
