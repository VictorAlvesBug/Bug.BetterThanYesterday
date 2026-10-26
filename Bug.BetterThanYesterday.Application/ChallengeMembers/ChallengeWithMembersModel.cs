using Bug.BetterThanYesterday.Application.Challenges;
using Bug.BetterThanYesterday.Application.Users;

namespace Bug.BetterThanYesterday.Application.ChallengeMembers;

public class ChallengeWithMembersModel
{
	public ChallengeModel Challenge { get; set; }
	public List<UserModel> Users { get; set; }
}
