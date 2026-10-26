using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Penalties.ValueObjects;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.Utils;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Domain.Penalties.Entities;

public sealed class Penalty : Entity
{
	public Guid ChallengeId { get; set; }
	public Guid UserId { get; set; }
	public DateOnly Date { get; set; }
	public decimal Amount { get; set; }
	public PenaltyStatus Status { get; set; }
	public PenaltyReason Reason { get; set; }
	public DateTime? PaidAt { get; set; }
	public PaymentMethod? PaymentMethod { get; set; }
	public string? ExternalPaymentId { get; set; }
	public Guid? ConfirmedByUserId { get; set; }

	private Penalty(
		Guid id,
		Guid challengeId,
		Guid userId,
		DateOnly date,
		decimal amount,
		string status,
		string reason,
		DateTime createdAt,
		DateTime? paidAt,
		string? paymentMethod,
		string? externalPaymentId,
		Guid? confirmedByUserId)
	{
		Id = id;
		ChallengeId = challengeId;
		UserId = userId;
		Date = date;
		Amount = amount;
		Status = PenaltyStatus.Get(status);
		Reason = PenaltyReason.Get(reason);
		CreatedAt = createdAt;
		PaidAt = paidAt;
		PaymentMethod = paymentMethod is null ? null : PaymentMethod.Get(paymentMethod);
		ExternalPaymentId = externalPaymentId;
		ConfirmedByUserId = confirmedByUserId;
	}

	public static Penalty CreateNew(
		Guid challengeId,
		Guid userId,
		DateOnly date,
		decimal amount)
	{
		if (challengeId == Guid.Empty)
			throw new ArgumentNullException(nameof(challengeId), Messages.EnterChallengeId);

		if (userId == Guid.Empty)
			throw new ArgumentNullException(nameof(userId), Messages.EnterUserId);

		if (amount <= 0)
			throw new ArgumentException(Messages.EnterValidPenaltyValue, nameof(amount));

		return new Penalty(
			BuildId(challengeId, userId, date),
			challengeId,
			userId,
			date,
			amount,
			PenaltyStatus.Pending.Name,
			PenaltyReason.MissedCheckIn.Name,
			DateTime.Now,
			null,
			null,
			null,
			null);
	}

	public static Penalty Restore(
		Guid id,
		Guid challengeId,
		Guid userId,
		DateOnly date,
		decimal amount,
		string status,
		string reason,
		DateTime createdAt,
		DateTime? paidAt,
		string? paymentMethod,
		string? externalPaymentId,
		Guid? confirmedByUserId)
	{
		if (id == Guid.Empty)
			throw new ArgumentNullException(nameof(id), Messages.EnterPenaltyId);

		return new Penalty(
			id,
			challengeId,
			userId,
			date,
			amount,
			status,
			reason,
			createdAt,
			paidAt,
			paymentMethod,
			externalPaymentId,
			confirmedByUserId);
	}

	public static Guid BuildId(Guid challengeId, Guid userId, DateOnly date)
	{
		var dateGuid = GuidFromDayNumber(date.DayNumber);
		return challengeId.Combine(userId).Combine(dateGuid);
	}

	public void MarkAsPaid(PaymentMethod paymentMethod, Guid? confirmedByUserId, string? externalPaymentId = null)
	{
		if (Status == PenaltyStatus.Paid)
			throw new InvalidOperationException(Messages.PenaltyAlreadyPaid);

		Status = PenaltyStatus.Paid;
		PaidAt = DateTime.Now;
		PaymentMethod = paymentMethod;
		ConfirmedByUserId = confirmedByUserId;
		ExternalPaymentId = externalPaymentId ?? ExternalPaymentId;
	}

	public void AssignCharge(string externalPaymentId)
	{
		if (Status == PenaltyStatus.Paid)
			throw new InvalidOperationException(Messages.PenaltyAlreadyPaid);

		if (string.IsNullOrWhiteSpace(externalPaymentId))
			throw new ArgumentException(Messages.EnterExternalPaymentId, nameof(externalPaymentId));

		ExternalPaymentId = externalPaymentId;
	}

	public void Forgive()
	{
		if (Status != PenaltyStatus.Pending)
			return;

		Status = PenaltyStatus.Forgiven;
	}

	private static Guid GuidFromDayNumber(int dayNumber)
	{
		var bytes = new byte[16];
		BitConverter.TryWriteBytes(bytes, dayNumber);
		return new Guid(bytes);
	}
}
