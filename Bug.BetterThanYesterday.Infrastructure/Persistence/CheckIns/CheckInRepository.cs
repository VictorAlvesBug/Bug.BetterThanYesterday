using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;
using MongoDB.Driver;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.CheckIns;

public class CheckInRepository(
	IMongoCollection<CheckInDocument> collection,
	IDocumentMapper<CheckIn, CheckInDocument> mapper)
	: Repository<CheckIn, CheckInDocument>(
		collection,
		mapper), ICheckInRepository
{
	public async Task<List<CheckIn>> ListByPlanIdAndUserIdAsync(Guid planId, Guid userId)
	{
		return (await _collection.FindAsync(checkIn =>
				checkIn.PlanId == planId 
				&& checkIn.UserId == userId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}

	public async Task<List<CheckIn>> ListByPlanIdAsync(Guid planId)
	{
		return (await _collection.FindAsync(checkIn => checkIn.PlanId == planId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}
}
