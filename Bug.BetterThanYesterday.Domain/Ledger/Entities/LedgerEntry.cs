using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Ledger.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Domain.Ledger.Entities;

public sealed class LedgerEntry : Entity
{
	public Guid PlanId { get; private set; }
	public Guid UserId { get; private set; }
	public Guid? PenaltyId { get; private set; }
	public DateOnly? Date { get; private set; }
	public decimal Amount { get; private set; }
	public LedgerKind Kind { get; private set; } = null!;
	public LedgerStatus Status { get; private set; } = null!;
	public string? ExternalPaymentId { get; private set; }

	public static LedgerEntry CreateFinePaid(
		Guid planId,
		Guid userId,
		Guid penaltyId,
		DateOnly date,
		decimal amount) =>
		Create(planId, userId, penaltyId, date, amount, LedgerKind.FinePaid, LedgerStatus.Recorded);

	public static LedgerEntry CreateSettlement(Guid planId, Guid userId, decimal amount) =>
		Create(planId, userId, null, null, amount, LedgerKind.Settlement, LedgerStatus.Owed);

	public static LedgerEntry CreateRefund(
		Guid planId,
		Guid userId,
		Guid penaltyId,
		DateOnly date,
		decimal amount) =>
		Create(planId, userId, penaltyId, date, amount, LedgerKind.Refund, LedgerStatus.Owed);

	public static LedgerEntry Restore(
		Guid id,
		Guid planId,
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
			planId,
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
		Guid planId,
		Guid userId,
		Guid? penaltyId,
		DateOnly? date,
		decimal amount,
		LedgerKind kind,
		LedgerStatus status)
	{
		if (planId == Guid.Empty)
			throw new ArgumentNullException(nameof(planId), Messages.EnterPlanId);

		if (userId == Guid.Empty)
			throw new ArgumentNullException(nameof(userId), Messages.EnterUserId);

		if (amount <= 0)
			throw new ArgumentException(Messages.EnterValidPenaltyValue, nameof(amount));

		return new LedgerEntry
		{
			Id = Guid.NewGuid(),
			PlanId = planId,
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
