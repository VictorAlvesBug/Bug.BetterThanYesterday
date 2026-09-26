using Bug.BetterThanYesterday.Domain.Penalties.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Penalties;

internal sealed class PenaltyMapper : IDocumentMapper<Penalty, PenaltyDocument>
{
	public PenaltyDocument ToDocument(Penalty penalty) => new()
	{
		Id = penalty.Id,
		PlanId = penalty.PlanId,
		UserId = penalty.UserId,
		Date = penalty.Date.ToDateTime(TimeOnly.MinValue),
		Amount = penalty.Amount,
		Status = penalty.Status.Name,
		Reason = penalty.Reason.Name,
		CreatedAt = penalty.CreatedAt,
		PaidAt = penalty.PaidAt,
		PaymentMethod = penalty.PaymentMethod?.Name,
		ExternalPaymentId = penalty.ExternalPaymentId,
		ConfirmedByUserId = penalty.ConfirmedByUserId,
	};

	public Penalty ToDomain(PenaltyDocument document) =>
		Penalty.Restore(
			document.Id,
			document.PlanId,
			document.UserId,
			DateOnly.FromDateTime(document.Date),
			document.Amount,
			document.Status,
			document.Reason,
			document.CreatedAt,
			document.PaidAt,
			document.PaymentMethod,
			document.ExternalPaymentId,
			document.ConfirmedByUserId);
}
