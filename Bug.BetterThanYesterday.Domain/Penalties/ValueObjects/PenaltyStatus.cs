using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Domain.Penalties.ValueObjects;

public sealed class PenaltyStatus : Enumeration<PenaltyStatus>
{
	public static readonly PenaltyStatus Pending = new(1, nameof(Pending));
	public static readonly PenaltyStatus Paid = new(2, nameof(Paid));
	public static readonly PenaltyStatus Forgiven = new(3, nameof(Forgiven));

	private PenaltyStatus(int id, string name) : base(id, name) { }
}
