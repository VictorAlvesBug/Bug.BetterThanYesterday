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
    public async Task<CheckIn> GetDetailsAsync(Guid challengeId, Guid userId, DateOnly date, int index = 0)
    {
		var dayStart = date.ToDateTime(TimeOnly.MinValue);
		var dayEnd = date.ToDateTime(TimeOnly.MaxValue);

		return mapper.ToDomain(
			(await _collection.FindAsync(checkInDoc =>
			checkInDoc.ChallengeId == challengeId
			&& checkInDoc.UserId == userId
			&& checkInDoc.Date >= dayStart
			&& checkInDoc.Date <= dayEnd
			&& checkInDoc.Index == index))
			.FirstOrDefault()
		);
    }

	public async Task<List<CheckIn>> ListByChallengeIdAsync(Guid challengeId)
	{
		return (await _collection.FindAsync(checkInDoc => checkInDoc.ChallengeId == challengeId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}

	public async Task<List<CheckIn>> ListByChallengeIdAndUserIdAsync(Guid challengeId, Guid userId)
	{
		return (await _collection.FindAsync(checkInDoc =>
				checkInDoc.ChallengeId == challengeId 
				&& checkInDoc.UserId == userId))
			.ToList()
			.ConvertAll(mapper.ToDomain);
	}

    public async Task<List<CheckIn>> ListByChallengeIdAndUserIdAndDateAsync(Guid challengeId, Guid userId, DateOnly date)
    {
		var dayStart = date.ToDateTime(TimeOnly.MinValue);
		var dayEnd = date.ToDateTime(TimeOnly.MaxValue);

		return (await _collection.FindAsync(checkInDoc =>
				checkInDoc.ChallengeId == challengeId 
				&& checkInDoc.UserId == userId
				&& checkInDoc.Date >= dayStart
				&& checkInDoc.Date <= dayEnd))
			.ToList()
			.ConvertAll(mapper.ToDomain);
    }
}
