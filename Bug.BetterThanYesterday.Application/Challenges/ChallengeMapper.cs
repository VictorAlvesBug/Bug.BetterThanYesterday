using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.Challenges.Entities;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;
using Bug.BetterThanYesterday.Domain.Users.Entities;

namespace Bug.BetterThanYesterday.Application.Challenges;

internal static class ChallengeMapper
{
	public static ChallengeModel ToModel(this Challenge challenge, Habit habit, User owner, int? memberCount = null) => new()
	{
		Id = challenge.Id,
		OwnerId = challenge.OwnerId,
		OwnerName = owner.Nickname,
		HabitId = challenge.HabitId,
		HabitName = habit.Name,
		Description = challenge.Description,
		StartsAt = challenge.StartsAt.ToDateTime(TimeOnly.MinValue),
		EndsAt = challenge.EndsAt.ToDateTime(TimeOnly.MinValue),
		Status = (memberCount is null ? challenge.GetStatus() : challenge.GetStatus(memberCount.Value)).Name,
		Type = challenge.Type.Name,
		DaysOffPerWeek = challenge.DaysOffPerWeek,
		PenaltyValue = challenge.PenaltyValue,
		CoverPhotoUrl = challenge.CoverPhotoUrl,
		CreatedAt = challenge.CreatedAt
	};
}
