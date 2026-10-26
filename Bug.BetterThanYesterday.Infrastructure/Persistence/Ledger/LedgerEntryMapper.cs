using Bug.BetterThanYesterday.Domain.Ledger.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Ledger;

internal sealed class LedgerEntryMapper : IDocumentMapper<LedgerEntry, LedgerEntryDocument>
{
	public LedgerEntryDocument ToDocument(LedgerEntry entry) => new()
	{
		Id = entry.Id,
		ChallengeId = entry.ChallengeId,
		UserId = entry.UserId,
		PenaltyId = entry.PenaltyId,
		Date = entry.Date?.ToDateTime(TimeOnly.MinValue),
		Amount = entry.Amount,
		Kind = entry.Kind.Name,
		Status = entry.Status.Name,
		ExternalPaymentId = entry.ExternalPaymentId,
		CreatedAt = entry.CreatedAt
	};

	public LedgerEntry ToDomain(LedgerEntryDocument document) =>
		LedgerEntry.Restore(
			document.Id,
			document.ChallengeId,
			document.UserId,
			document.PenaltyId,
			document.Date,
			document.Amount,
			document.Kind,
			document.Status,
			document.ExternalPaymentId,
			document.CreatedAt);
}
