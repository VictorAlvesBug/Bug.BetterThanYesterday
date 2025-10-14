using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Application.Users;

namespace Bug.BetterThanYesterday.Application.PlanParticipants;

public class PlanWithParticipantsModel
{
	public PlanModel Plan { get; set; }
	public List<UserModel> Participants { get; set; }
}
