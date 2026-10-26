using Bug.BetterThanYesterday.Application.Challenges;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.Challenges.Entities;
using Bug.BetterThanYesterday.Domain.Users.Entities;

namespace Bug.BetterThanYesterday.Application.Habits;

public static class HabitMapper
{
	public static HabitModel ToModel(
		this Habit habit,
		List<Challenge>? challenges = null,
		List<User>? owners = null) => new()
	{
		Id = habit.Id,
		Name = habit.Name,
		CreatedAt = habit.CreatedAt,
		Challenges = challenges is null 
			? [] 
			: challenges.Select(challenge => challenge.ToModel(
				habit,
				owners!.First(owner => owner.Id == challenge.OwnerId))).ToList()
	};
}
