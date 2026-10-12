using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Domain.Ledger.ValueObjects;

public sealed class LedgerStatus : Enumeration<LedgerStatus>
{
	public static readonly LedgerStatus Recorded = new(1, nameof(Recorded));
	public static readonly LedgerStatus Owed = new(2, nameof(Owed));
	public static readonly LedgerStatus Paid = new(3, nameof(Paid));

	private LedgerStatus(int id, string name) : base(id, name) { }
}
