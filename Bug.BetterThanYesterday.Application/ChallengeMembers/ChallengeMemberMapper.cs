using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.Challenges;
using Bug.BetterThanYesterday.Application.Users;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.Entities;
using Bug.BetterThanYesterday.Domain.Challenges.Entities;
using Bug.BetterThanYesterday.Domain.Users.Entities;

namespace Bug.BetterThanYesterday.Application.ChallengeMembers;

internal static class ChallengeMemberMapper
{
	public static ChallengeMemberDetailsModel ToChallengeMemberDetailsModel(
		this ChallengeMember challengeMember,
		Habit habit,
		Challenge challenge,
		User owner,
		User user) => new()
		{
			Id = challengeMember.Id,
			JoinedAt = challengeMember.JoinedAt.ToDateTime(TimeOnly.MinValue),
			Status = challengeMember.Status.Name,
			Challenge = challenge.ToModel(habit, owner),
			User = user.ToModel(),
		};

	public static ChallengeWithMembersModel ToChallengeWithMembersModel(
		this Challenge challenge,
		Habit habit,
		User owner,
		List<User>? users = null) => new()
		{
			Challenge = challenge.ToModel(habit, owner),
			Users = users?.Select(user => user.ToModel()).ToList() ?? [],
		};

	public static UserWithChallengesModel ToUserWithChallengesModel(
		this User user,
		List<ChallengeModel>? challenges = null) => new()
		{
			User = user.ToModel(),
			Challenges = challenges ?? [],
		};
}
