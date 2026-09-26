using Bug.BetterThanYesterday.Application.Penalties.Models;
using Bug.BetterThanYesterday.Domain.Penalties.Entities;

namespace Bug.BetterThanYesterday.Application.Penalties;

public static class PenaltyMapper
{
	public static PenaltyModel ToModel(Penalty penalty) => new()
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
}
