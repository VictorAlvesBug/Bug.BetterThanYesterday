using Bug.BetterThanYesterday.Domain.Habits.Policies;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;

namespace Bug.BetterThanYesterday.Application.Habits.DeleteHabit;

public class HabitDeletionPolicy(IChallengeRepository challengeRepository)
	: IHabitDeletionPolicy
{
	public async Task<bool> CanDeleteAsync(Guid habitId)
	{
		var challenges = await challengeRepository.ListByHabitIdAsync(habitId);
		return !challenges.Any(challenge => challenge.GetStatus() != ChallengeStatus.Cancelled);
	}
}
