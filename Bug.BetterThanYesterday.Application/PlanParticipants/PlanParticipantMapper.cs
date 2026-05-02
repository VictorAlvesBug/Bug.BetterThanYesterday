using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Application.Users;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Users.Entities;

namespace Bug.BetterThanYesterday.Application.PlanParticipants;

internal static class PlanParticipantMapper
{
	public static PlanParticipantDetailsModel ToPlanParticipantDetailsModel(
		this PlanParticipant planParticipant,
		Habit habit,
		Plan plan,
		User participant) => new()
		{
			PlanParticipantId = planParticipant.Id,
			JoinedAt = planParticipant.JoinedAt.ToDateTime(TimeOnly.MinValue),
			LeftAt = planParticipant.LeftAt?.ToDateTime(TimeOnly.MinValue),
			StatusId = planParticipant.Status.Id,
			StatusName = planParticipant.Status.Name,
			Plan = plan.ToModel(habit.ToModel()),
			Participant = participant.ToModel(),
		};

	public static PlanWithParticipantsModel ToPlanWithParticipantsModel(
		this Plan plan,
		Habit habit,
		List<User>? participants = null) => new()
		{
			Plan = plan.ToModel(habit.ToModel()),
			Participants = participants?.Select(participant => participant.ToModel()).ToList() ?? [],
		};

	public static UserWithPlansModel ToUserWithPlansModel(
		this User user,
		List<PlanModel>? plans = null) => new()
		{
			User = user.ToModel(),
			Plans = plans ?? [],
		};
}
