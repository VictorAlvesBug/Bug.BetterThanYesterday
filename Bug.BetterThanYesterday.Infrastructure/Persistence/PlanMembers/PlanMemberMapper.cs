using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.PlanMembers;

internal sealed class PlanMemberMapper : IDocumentMapper<PlanMember, PlanMemberDocument>
{
	public PlanMemberDocument ToDocument(PlanMember planMember) => new()
	{
		Id = planMember.Id,
		PlanId = planMember.PlanId,
		UserId = planMember.UserId,
		JoinedAt = planMember.JoinedAt.ToDateTime(TimeOnly.MinValue),
		LeftAt = planMember.LeftAt?.ToDateTime(TimeOnly.MinValue),
		StatusId = planMember.Status.Id,
		CreatedAt = planMember.CreatedAt.ToDateTime(TimeOnly.MinValue)
	};

	public PlanMember ToDomain(PlanMemberDocument document) => PlanMember.Restore(
		document.Id,
		document.PlanId,
		document.UserId,
		document.JoinedAt,
		document.LeftAt,
		document.StatusId,
		document.CreatedAt
	);
}
