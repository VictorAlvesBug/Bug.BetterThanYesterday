using Bug.BetterThanYesterday.Domain.Ledger;
using Bug.BetterThanYesterday.Domain.Ledger.Entities;
using Bug.BetterThanYesterday.Domain.Penalties.Entities;

namespace Bug.BetterThanYesterday.Application.Ledger;

public static class FinePaymentBook
{
	public static async Task RecordAsync(ILedgerRepository ledgerRepository, Penalty penalty)
	{
		var entries = await ledgerRepository.ListByPlanIdAsync(penalty.PlanId);

		if (entries.Any(entry => entry.Kind == Domain.Ledger.ValueObjects.LedgerKind.FinePaid && entry.PenaltyId == penalty.Id))
			return;

		await ledgerRepository.AddAsync(LedgerEntry.CreateFinePaid(
			penalty.PlanId,
			penalty.UserId,
			penalty.Id,
			penalty.Date,
			penalty.Amount));
	}
}
