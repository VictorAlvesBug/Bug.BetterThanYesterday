using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.Configurations;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;
using MongoDB.Driver;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.CheckIns
{
	public class CheckInRepository(IDatabaseConfig databaseConfig)
		: Repository<CheckIn>(databaseConfig, "checkins"), ICheckInRepository
	{
		public async Task<List<CheckIn>> ListByPlanIdAndUserIdAsync(string planId, string userId)
		{
			return (await _entities.FindAsync(checkIn => checkIn.PlanId == planId && checkIn.UserId == userId)).ToList();
		}

		public async Task<List<CheckIn>> ListByPlanIdAsync(string planId)
		{
			return (await _entities.FindAsync(checkIn => checkIn.PlanId == planId)).ToList();
		}
	}
}
