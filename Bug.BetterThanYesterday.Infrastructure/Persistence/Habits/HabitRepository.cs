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
	public async Task<Habit?> GetByNameAsync(string name)
	{
		var document = (await _collection.FindAsync(user => user.Name == name)).FirstOrDefault();
		return document is null ? null : _mapper.ToDomain(document);
	}
}
