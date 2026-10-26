using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Challenges.Entities;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;

namespace Bug.BetterThanYesterday.Application.Challenges;

public static class ChallengeQuorumPostponer
{
	public static async Task PostponeWhileUnderQuorumAsync(Challenge challenge, int memberCount, IChallengeRepository challengeRepository)
	{
		if (challenge.GetStatus() != ChallengeStatus.Running || memberCount >= 2)
			return;

		var today = DateOnly.FromDateTime(DateTime.Today);
		var shift = today.DayNumber - challenge.StartsAt.DayNumber + 1;
		if (shift < 1)
			return;

		challenge.UpdatePeriod(challenge.StartsAt.AddDays(shift), challenge.EndsAt.AddDays(shift), memberCount);
		await challengeRepository.UpdateAsync(challenge);
	}
}
