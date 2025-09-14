using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.Configurations;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;
using MongoDB.Driver;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.CheckIns;

public class CheckInRepository(
	IDatabaseConfig databaseConfig,
	IDocumentMapper<CheckIn, CheckInDocument> mapper)
	: Repository<CheckIn, CheckInDocument>(
		databaseConfig,
		"checkins",
		mapper), ICheckInRepository
{
	public async Task<List<CheckIn>> ListByPlanIdAndUserIdAsync(string planId, string userId)
	{
		return (await _collection.FindAsync(checkIn => checkIn.PlanId == planId && checkIn.UserId == userId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}

	public async Task<List<CheckIn>> ListByPlanIdAsync(string planId)
	{
		return (await _collection.FindAsync(checkIn => checkIn.PlanId == planId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}
}
