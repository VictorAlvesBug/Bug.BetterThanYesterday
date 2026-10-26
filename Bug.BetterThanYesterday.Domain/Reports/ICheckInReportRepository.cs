using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Reports.Entities;

namespace Bug.BetterThanYesterday.Domain.Reports;

public interface ICheckInReportRepository : IRepository<CheckInReport>
{
	Task<List<CheckInReport>> ListByChallengeIdAsync(Guid challengeId);
}
