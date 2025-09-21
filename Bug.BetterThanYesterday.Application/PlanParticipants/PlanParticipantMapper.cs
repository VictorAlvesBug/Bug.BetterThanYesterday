using Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;

namespace Bug.BetterThanYesterday.Application.PlanParticipants;

internal static class PlanParticipantMapper
{
	public static PlanParticipantModel ToModel(this PlanParticipant planParticipant) => new()
	{
		Id = planParticipant.Id,
		UserId = planParticipant.UserId,
		PlanId = planParticipant.PlanId,
		JoinedAt = planParticipant.JoinedAt.ToDateTime(TimeOnly.MinValue),
		LeftAt = planParticipant.LeftAt?.ToDateTime(TimeOnly.MinValue),
		StatusId = planParticipant.Status.Id,
		StatusName = planParticipant.Status.Name
	};
}
