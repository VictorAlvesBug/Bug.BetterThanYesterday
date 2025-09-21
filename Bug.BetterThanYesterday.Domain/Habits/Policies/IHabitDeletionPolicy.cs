namespace Bug.BetterThanYesterday.Domain.Habits.Policies;

public interface IHabitDeletionPolicy
{
	Task<bool> CanDeleteAsync(Guid habitId);
}
