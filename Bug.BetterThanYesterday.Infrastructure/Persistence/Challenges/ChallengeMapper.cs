using Bug.BetterThanYesterday.Domain.Challenges.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Challenges;

internal sealed class ChallengeMapper : IDocumentMapper<Challenge, ChallengeDocument>
{
	public ChallengeDocument ToDocument(Challenge challenge) => new()
	{
		Id = challenge.Id,
		OwnerId = challenge.OwnerId,
		HabitId = challenge.HabitId,
		Description = challenge.Description,
		StartsAt = challenge.StartsAt.ToDateTime(TimeOnly.MinValue),
		EndsAt = challenge.EndsAt.ToDateTime(TimeOnly.MinValue),
		Type = challenge.Type.Name,
		DaysOffPerWeek = challenge.DaysOffPerWeek,
		PenaltyValue = challenge.PenaltyValue,
		IsCancelled = challenge.IsCancelled,
		CoverPhotoUrl = challenge.CoverPhotoUrl,
		CreatedAt = challenge.CreatedAt,
	};

	public Challenge ToDomain(ChallengeDocument document) => Challenge.Restore(
		document.Id,
		document.OwnerId,
		document.HabitId,
		document.Description,
		document.StartsAt,
		document.EndsAt,
		document.Type,
		document.DaysOffPerWeek,
		document.PenaltyValue,
		document.IsCancelled,
		document.CreatedAt,
		document.CoverPhotoUrl);
}
