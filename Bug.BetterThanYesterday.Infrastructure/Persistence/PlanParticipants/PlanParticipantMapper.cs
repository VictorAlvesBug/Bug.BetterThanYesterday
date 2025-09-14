using Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;
using Bug.BetterThanYesterday.Domain.PlanParticipants.ValueObjects;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.PlanParticipants;

internal sealed class PlanParticipantMapper : IDocumentMapper<PlanParticipant, PlanParticipantDocument>
{
	public PlanParticipantDocument ToDocument(PlanParticipant planParticipant) => new()
	{
		Id = planParticipant.Id,
		UserId = planParticipant.UserId,
		PlanId = planParticipant.PlanId,
		JoinedAt = planParticipant.JoinedAt,
		LeftAt = planParticipant.LeftAt,
		Status = planParticipant.Status
	};

	public PlanParticipant ToDomain(PlanParticipantDocument document) => PlanParticipant.Restore(
		document.Id,
		document.UserId,
		document.PlanId,
		document.JoinedAt,
		document.LeftAt,
		(PlanParticipantStatus) document.Status
	);
}
