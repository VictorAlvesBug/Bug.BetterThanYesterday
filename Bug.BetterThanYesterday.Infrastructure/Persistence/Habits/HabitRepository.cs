using Bug.BetterThanYesterday.Domain.Configurations;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Habits
{
	public class HabitRepository(IDatabaseConfig databaseConfig)
		: Repository<Habit>(databaseConfig, "habits"), IHabitRepository
	{
	}
}
