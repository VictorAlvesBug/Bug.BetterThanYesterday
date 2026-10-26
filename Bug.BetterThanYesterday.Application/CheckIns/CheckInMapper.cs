using Bug.BetterThanYesterday.Application.CheckIns.Models;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Challenges.Entities;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;

namespace Bug.BetterThanYesterday.Application.CheckIns;

internal static class CheckInMapper
{
	public static CheckInModel ToModel(
		this CheckIn checkIn,
		Challenge challenge,
		Habit habit,
		User user) => new()
		{
			Id = checkIn.Id,
			ChallengeId = checkIn.ChallengeId,
			ChallengeName = challenge.Description ?? habit.Name,
			UserId = checkIn.UserId,
			UserName = user.Nickname,
			UserPhotoUrl = user.Photo?.Value,
			Date = checkIn.Date,
			Index = checkIn.Index,
			Title = checkIn.Title,
			EvidencePhotoUrl = checkIn.PhotoUrl,
			Status = checkIn.ResolveStatus().Name,
			Reviews = checkIn.Reviews.Select(review => review.ToModel()).ToArray(),
			CreatedAt = checkIn.CreatedAt,
			ReviewClosesAt = checkIn.ReviewClosesAt,
			IsReplacement = checkIn.IsReplacement
		};

	public static async Task<CheckInModel> ToModelAsync(
		this CheckIn checkIn,
		IChallengeRepository challengeRepository,
		IHabitRepository habitRepository,
		IUserRepository userRepository
	)
	{
		var challenge = await challengeRepository.GetByIdAsync(checkIn.ChallengeId);

        if (challenge is null)
            throw new Exception(Messages.ChallengeNotFound);

        var habit = await habitRepository.GetByIdAsync(challenge.HabitId);

        if (habit is null)
            throw new Exception(Messages.HabitNotFound);

        var user = await userRepository.GetByIdAsync(checkIn.UserId);

        if (user is null)
            throw new Exception(Messages.UserNotFound);

		return checkIn.ToModel(challenge, habit, user);
	}
}
