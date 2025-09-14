using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.Configurations;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.CheckIns;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Habits;

public class HabitRepository(
	IDatabaseConfig databaseConfig,
	IDocumentMapper<Habit, HabitDocument> mapper)
	: Repository<Habit, HabitDocument>(
		databaseConfig,
		"habits",
		mapper), IHabitRepository
{
}
