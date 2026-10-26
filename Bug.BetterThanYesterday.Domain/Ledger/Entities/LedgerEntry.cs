using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Ledger.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Domain.Ledger.Entities;

public sealed class LedgerEntry : Entity
{
	public Guid ChallengeId { get; private set; }
	public Guid UserId { get; private set; }
	public Guid? PenaltyId { get; private set; }
	public DateOnly? Date { get; private set; }
	public decimal Amount { get; private set; }
	public LedgerKind Kind { get; private set; } = null!;
	public LedgerStatus Status { get; private set; } = null!;
	public string? ExternalPaymentId { get; private set; }

	public static LedgerEntry CreateFinePaid(
		Guid challengeId,
		Guid userId,
		Guid penaltyId,
		DateOnly date,
		decimal amount) =>
		Create(challengeId, userId, penaltyId, date, amount, LedgerKind.FinePaid, LedgerStatus.Recorded);

	public static LedgerEntry CreateSettlement(Guid challengeId, Guid userId, decimal amount) =>
		Create(challengeId, userId, null, null, amount, LedgerKind.Settlement, LedgerStatus.Owed);

	public static LedgerEntry CreateRefund(
		Guid challengeId,
		Guid userId,
		Guid penaltyId,
		DateOnly date,
		decimal amount) =>
		Create(challengeId, userId, penaltyId, date, amount, LedgerKind.Refund, LedgerStatus.Owed);

	public static LedgerEntry Restore(
		Guid id,
		Guid challengeId,
		Guid userId,
		Guid? penaltyId,
		DateTime? date,
		decimal amount,
		string kind,
		string status,
		string? externalPaymentId,
		DateTime createdAt)
	{
		var entry = Create(
			challengeId,
			userId,
			penaltyId,
			date is null ? null : DateOnly.FromDateTime(date.Value),
			amount,
			LedgerKind.Get(kind),
			LedgerStatus.Get(status));
		entry.Id = id;
		entry.CreatedAt = createdAt;
		entry.ExternalPaymentId = externalPaymentId;
		return entry;
	}

	public void MarkPaid(string externalPaymentId)
	{
		if (Status != LedgerStatus.Owed)
			return;

		Status = LedgerStatus.Paid;
		ExternalPaymentId = externalPaymentId;
	}

	private static LedgerEntry Create(
		Guid challengeId,
		Guid userId,
		Guid? penaltyId,
		DateOnly? date,
		decimal amount,
		LedgerKind kind,
		LedgerStatus status)
	{
		if (challengeId == Guid.Empty)
			throw new ArgumentNullException(nameof(challengeId), Messages.EnterChallengeId);

		if (userId == Guid.Empty)
			throw new ArgumentNullException(nameof(userId), Messages.EnterUserId);

		if (amount <= 0)
			throw new ArgumentException(Messages.EnterValidPenaltyValue, nameof(amount));

		return new LedgerEntry
		{
			Id = Guid.NewGuid(),
			ChallengeId = challengeId,
			UserId = userId,
			PenaltyId = penaltyId,
			Date = date,
			Amount = amount,
			Kind = kind,
			Status = status,
			CreatedAt = DateTime.Now
		};
	}
}
