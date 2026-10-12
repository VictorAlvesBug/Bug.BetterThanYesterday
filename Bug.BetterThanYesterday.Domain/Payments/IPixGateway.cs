namespace Bug.BetterThanYesterday.Domain.Payments;

public sealed record PixChargeResult(string ExternalId, string CopyAndPaste);

public sealed record PixTransferResult(string ExternalId);

public interface IPixGateway
{
	Task<PixChargeResult> CreateChargeAsync(decimal amount, string description, Guid referenceId);
	Task<PixTransferResult> SendAsync(string pixKey, decimal amount, string description);
}
