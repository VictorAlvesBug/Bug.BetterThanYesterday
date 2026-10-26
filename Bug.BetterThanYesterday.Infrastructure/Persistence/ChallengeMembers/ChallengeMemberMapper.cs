using Bug.BetterThanYesterday.Domain.ChallengeMembers.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.ChallengeMembers;

internal sealed class ChallengeMemberMapper : IDocumentMapper<ChallengeMember, ChallengeMemberDocument>
{
	public ChallengeMemberDocument ToDocument(ChallengeMember challengeMember) => new()
	{
		Id = challengeMember.Id,
		ChallengeId = challengeMember.ChallengeId,
		UserId = challengeMember.UserId,
		JoinedAt = challengeMember.JoinedAt.ToDateTime(TimeOnly.MinValue),
		Status = challengeMember.Status.Name,
		IsDelinquent = challengeMember.IsDelinquent,
		CreatedAt = challengeMember.CreatedAt
	};

	public ChallengeMember ToDomain(ChallengeMemberDocument document) => ChallengeMember.Restore(
		document.Id,
		document.ChallengeId,
		document.UserId,
		document.JoinedAt,
		document.Status,
		document.CreatedAt,
		document.IsDelinquent
	);
}
