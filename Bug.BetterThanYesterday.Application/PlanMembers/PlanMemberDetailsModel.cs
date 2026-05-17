using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Application.Users;

namespace Bug.BetterThanYesterday.Application.PlanMembers;

public class PlanMemberDetailsModel
{
	public Guid PlanMemberId { get; set; }
	public DateTime JoinedAt { get; set; }
	public string Status { get; set; }
	public PlanModel Plan { get; set; }
	public UserModel Member { get; set; }
}
