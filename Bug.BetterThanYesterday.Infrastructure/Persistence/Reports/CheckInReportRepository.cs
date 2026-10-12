using Bug.BetterThanYesterday.Domain.Reports;
using Bug.BetterThanYesterday.Domain.Reports.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;
using MongoDB.Driver;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Reports;

public class CheckInReportRepository(
	IMongoCollection<CheckInReportDocument> collection,
	IDocumentMapper<CheckInReport, CheckInReportDocument> mapper)
	: Repository<CheckInReport, CheckInReportDocument>(collection, mapper), ICheckInReportRepository
{
	public async Task<List<CheckInReport>> ListByPlanIdAsync(Guid planId)
	{
		return (await _collection.FindAsync(doc => doc.PlanId == planId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}
}
