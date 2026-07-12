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
    public async Task<CheckIn> GetDetailsAsync(Guid planId, Guid userId, DateOnly date, int index = 0)
    {
		var dayStart = date.ToDateTime(TimeOnly.MinValue);
		var dayEnd = date.ToDateTime(TimeOnly.MaxValue);

		return mapper.ToDomain(
			(await _collection.FindAsync(checkInDoc =>
			checkInDoc.PlanId == planId
			&& checkInDoc.UserId == userId
			&& checkInDoc.Date >= dayStart
			&& checkInDoc.Date <= dayEnd
			&& checkInDoc.Index == index))
			.FirstOrDefault()
		);
    }

	public async Task<List<CheckIn>> ListByPlanIdAsync(Guid planId)
	{
		return (await _collection.FindAsync(checkInDoc => checkInDoc.PlanId == planId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}

	public async Task<List<CheckIn>> ListByPlanIdAndUserIdAsync(Guid planId, Guid userId)
	{
		return (await _collection.FindAsync(checkInDoc =>
				checkInDoc.PlanId == planId 
				&& checkInDoc.UserId == userId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}

    public async Task<List<CheckIn>> ListByPlanIdAndUserIdAndDateAsync(Guid planId, Guid userId, DateOnly date)
    {
		var dayStart = date.ToDateTime(TimeOnly.MinValue);
		var dayEnd = date.ToDateTime(TimeOnly.MaxValue);

		return (await _collection.FindAsync(checkInDoc =>
				checkInDoc.PlanId == planId 
				&& checkInDoc.UserId == userId
				&& checkInDoc.Date >= dayStart
				&& checkInDoc.Date <= dayEnd))
			.ToList()
			.ConvertAll(mapper.ToDomain);
    }
}
