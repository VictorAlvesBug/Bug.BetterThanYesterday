namespace Bug.BetterThanYesterday.Domain.Payments;

public interface IPixGateway
{
	Task<PixChargeResult> CreateChargeAsync(decimal amount, string description, Guid referenceId);
	Task<PixTransferResult> SendAsync(string pixKey, decimal amount, string description);
}
