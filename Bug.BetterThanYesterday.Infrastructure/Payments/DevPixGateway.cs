using Bug.BetterThanYesterday.Domain.Payments;

namespace Bug.BetterThanYesterday.Infrastructure.Payments;

public sealed class DevPixGateway : IPixGateway
{
	public Task<PixChargeResult> CreateChargeAsync(decimal amount, string description, Guid referenceId)
	{
		var externalId = referenceId.ToString("N");
		var cents = decimal.Round(amount * 100m, 0, MidpointRounding.AwayFromZero).ToString("0");
		var payload = $"00020126BR.GOV.BCB.PIX{externalId}54{cents}5802BR";
		return Task.FromResult(new PixChargeResult(externalId, payload));
	}

	public Task<PixTransferResult> SendAsync(string pixKey, decimal amount, string description)
	{
		return Task.FromResult(new PixTransferResult(Guid.NewGuid().ToString("N")));
	}
}
