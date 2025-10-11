using Bug.BetterThanYesterday.Domain.Habits.Entities;

namespace Bug.BetterThanYesterday.Application.Habits;

public static class HabitMapper
{
	public static HabitModel ToModel(this Habit habit) => new()
	{
		HabitId = habit.Id,
		Name = habit.Name,
		CreatedAt = habit.CreatedAt.ToDateTime(TimeOnly.MinValue)
	};
}
