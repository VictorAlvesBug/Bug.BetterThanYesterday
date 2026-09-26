using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Domain.Penalties.ValueObjects;

public sealed class PenaltyReason : Enumeration<PenaltyReason>
{
	public static readonly PenaltyReason MissedCheckIn = new(1, nameof(MissedCheckIn));

	private PenaltyReason(int id, string name) : base(id, name) { }
}
