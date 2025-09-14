using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Habits
{
	internal sealed class HabitMapper : IDocumentMapper<Habit, HabitDocument>
	{
		public HabitDocument ToDocument(Habit habit) => new()
		{
			Id = habit.Id,
			Name = habit.Name,
			CreatedAt = habit.CreatedAt
		};

		public Habit ToDomain(HabitDocument document) => Habit.Restore(
			document.Id,
			document.Name,
			document.CreatedAt);
	}
}
