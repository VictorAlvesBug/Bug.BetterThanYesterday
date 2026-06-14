using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Application.Users;

namespace Bug.BetterThanYesterday.Application.PlanMembers;

public class PlanWithMembersModel
{
	public PlanModel Plan { get; set; }
	public List<UserModel> Users { get; set; }
}
