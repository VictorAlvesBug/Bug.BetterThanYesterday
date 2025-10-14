using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Application.Users;

namespace Bug.BetterThanYesterday.Application.PlanParticipants;

public class UserWithPlansModel
{
	public UserModel User { get; set; }
	public List<PlanModel> Plans { get; set; }
}