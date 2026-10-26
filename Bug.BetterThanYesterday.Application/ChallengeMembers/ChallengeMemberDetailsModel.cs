using Bug.BetterThanYesterday.Application.Challenges;
using Bug.BetterThanYesterday.Application.Users;

namespace Bug.BetterThanYesterday.Application.ChallengeMembers;

public class ChallengeMemberDetailsModel
{
	public Guid Id { get; set; }
	public DateTime JoinedAt { get; set; }
	public string Status { get; set; }
	public ChallengeModel Challenge { get; set; }
	public UserModel User { get; set; }
}
