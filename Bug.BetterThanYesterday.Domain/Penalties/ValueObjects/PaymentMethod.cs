using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Domain.Penalties.ValueObjects;

public sealed class PaymentMethod : Enumeration<PaymentMethod>
{
	public static readonly PaymentMethod Manual = new(1, nameof(Manual));
	public static readonly PaymentMethod Pix = new(2, nameof(Pix));

	private PaymentMethod(int id, string name) : base(id, name) { }
}
