using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Habits.Entities;

namespace Bug.BetterThanYesterday.Application.Habits;

internal sealed class HabitMapper : IModelMapper<Habit, HabitModel>
{
	public HabitModel ToModel(Habit habit) => new()
	{
		Id = habit.Id,
		Name = habit.Name,
		CreatedAt = habit.CreatedAt
	};
}
