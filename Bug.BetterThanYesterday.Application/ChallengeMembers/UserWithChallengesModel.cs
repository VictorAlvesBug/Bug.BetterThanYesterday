using Bug.BetterThanYesterday.Application.Challenges;
using Bug.BetterThanYesterday.Application.Users;

namespace Bug.BetterThanYesterday.Application.ChallengeMembers;

public class UserWithChallengesModel
{
	public UserModel User { get; set; }
	public List<ChallengeModel> Challenges { get; set; }
}