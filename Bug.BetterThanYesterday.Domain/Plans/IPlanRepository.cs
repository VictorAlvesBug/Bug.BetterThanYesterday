using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Plans.Entities;

namespace Bug.BetterThanYesterday.Domain.Plans
{
	public interface IPlanRepository : IRepository<Plan>
	{
		Task<List<Plan>> ListByHabitIdAsync(string habitId);
	}
}
