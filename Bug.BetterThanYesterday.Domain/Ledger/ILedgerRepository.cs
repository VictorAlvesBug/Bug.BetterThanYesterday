using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Ledger.Entities;

namespace Bug.BetterThanYesterday.Domain.Ledger;

public interface ILedgerRepository : IRepository<LedgerEntry>
{
	Task<List<LedgerEntry>> ListByPlanIdAsync(Guid planId);
}
