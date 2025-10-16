using Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.PlanParticipants;

internal sealed class PlanParticipantMapper : IDocumentMapper<PlanParticipant, PlanParticipantDocument>
{
	public PlanParticipantDocument ToDocument(PlanParticipant planParticipant) => new()
	{
		Id = planParticipant.Id,
		PlanId = planParticipant.PlanId,
		UserId = planParticipant.UserId,
		JoinedAt = planParticipant.JoinedAt.ToDateTime(TimeOnly.MinValue),
		LeftAt = planParticipant.LeftAt?.ToDateTime(TimeOnly.MinValue),
		StatusId = planParticipant.Status.Id
	};

	public PlanParticipant ToDomain(PlanParticipantDocument document) => PlanParticipant.Restore(
		document.Id,
		document.PlanId,
		document.UserId,
		document.JoinedAt,
		document.LeftAt,
		document.StatusId
	);
}
