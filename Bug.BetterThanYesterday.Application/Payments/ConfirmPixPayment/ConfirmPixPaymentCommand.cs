using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Payments.ConfirmPixPayment;

public sealed class ConfirmPixPaymentCommand : ICommand
{
	public required string ExternalPaymentId { get; init; }

	public void Validate()
	{
		if (string.IsNullOrWhiteSpace(ExternalPaymentId))
			throw new ArgumentException(Messages.EnterExternalPaymentId, nameof(ExternalPaymentId));
	}
}
