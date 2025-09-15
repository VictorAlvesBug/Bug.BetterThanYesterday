using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;
using MongoDB.Driver;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Habits;

public class HabitRepository(
	IMongoCollection<HabitDocument> collection,
	IDocumentMapper<Habit, HabitDocument> mapper)
	: Repository<Habit, HabitDocument>(
		collection,
		mapper), IHabitRepository
{
}
