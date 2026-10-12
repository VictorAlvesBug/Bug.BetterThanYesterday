using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Domain.Ledger.ValueObjects;

public sealed class LedgerKind : Enumeration<LedgerKind>
{
	// Multa Paga
	public static readonly LedgerKind FinePaid = new(1, nameof(FinePaid));
	// Recompensa
	public static readonly LedgerKind Settlement = new(2, nameof(Settlement));
	// Reembolso
	public static readonly LedgerKind Refund = new(3, nameof(Refund));

	private LedgerKind(int id, string name) : base(id, name) { }
}
