using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Application.Users;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Users.Entities;

namespace Bug.BetterThanYesterday.Application.PlanMembers;

internal static class PlanMemberMapper
{
	public static PlanMemberDetailsModel ToPlanMemberDetailsModel(
		this PlanMember planMember,
		Habit habit,
		Plan plan,
		User member) => new()
		{
			PlanMemberId = planMember.Id,
			JoinedAt = planMember.JoinedAt.ToDateTime(TimeOnly.MinValue),
			LeftAt = planMember.LeftAt?.ToDateTime(TimeOnly.MinValue),
			StatusId = planMember.Status.Id,
			StatusName = planMember.Status.Name,
			Plan = plan.ToModel(habit.ToModel()),
			Member = member.ToModel(),
		};

	public static PlanWithMembersModel ToPlanWithMembersModel(
		this Plan plan,
		Habit habit,
		List<User>? members = null) => new()
		{
			Plan = plan.ToModel(habit.ToModel()),
			Members = members?.Select(member => member.ToModel()).ToList() ?? [],
		};

	public static UserWithPlansModel ToUserWithPlansModel(
		this User user,
		List<PlanModel>? plans = null) => new()
		{
			User = user.ToModel(),
			Plans = plans ?? [],
		};
}
