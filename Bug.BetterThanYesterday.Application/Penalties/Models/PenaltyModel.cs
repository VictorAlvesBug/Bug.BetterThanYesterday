namespace Bug.BetterThanYesterday.Application.Penalties.Models;

public sealed class PenaltyModel
{
	public Guid Id { get; set; }
	public Guid ChallengeId { get; set; }
	public Guid UserId { get; set; }
	public DateTime Date { get; set; }
	public decimal Amount { get; set; }
	public string Status { get; set; } = string.Empty;
	public string Reason { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; }
	public DateTime? PaidAt { get; set; }
	public string? PaymentMethod { get; set; }
	public string? ExternalPaymentId { get; set; }
	public Guid? ConfirmedByUserId { get; set; }
}
