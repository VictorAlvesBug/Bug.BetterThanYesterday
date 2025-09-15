using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;

namespace Bug.BetterThanYesterday.Application.PlanParticipants;

internal sealed class PlanParticipantMapper : IModelMapper<PlanParticipant, PlanParticipantModel>
{
	public PlanParticipantModel ToModel(PlanParticipant planParticipant) => new()
	{
		Id = planParticipant.Id,
		UserId = planParticipant.UserId,
		PlanId = planParticipant.PlanId,
		JoinedAt = planParticipant.JoinedAt,
		LeftAt = planParticipant.LeftAt,
		Status = planParticipant.Status
	};
}
