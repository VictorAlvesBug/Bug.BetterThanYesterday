using Bug.BetterThanYesterday.Application.CheckIns.Models;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;

namespace Bug.BetterThanYesterday.Application.CheckIns;

internal static class CheckInMapper
{
	public static CheckInModel ToModel(
		this CheckIn checkIn,
		Plan plan,
		Habit habit,
		User user) => new()
		{
			Id = checkIn.Id,
			PlanId = checkIn.PlanId,
			PlanName = plan.Description ?? habit.Name,
			UserId = checkIn.UserId,
			UserName = user.Name,
			Date = checkIn.Date.ToDateTime(TimeOnly.MinValue),
			Index = checkIn.Index,
			Title = checkIn.Title,
			PhotoUrl = checkIn.PhotoUrl,
			Status = checkIn.Status.Name,
			Reviews = checkIn.Reviews.Select(review => review.ToModel()).ToArray(),
			CreatedAt = checkIn.CreatedAt
		};

	public static async Task<CheckInModel> ToModelAsync(
		this CheckIn checkIn,
		IPlanRepository planRepository,
		IHabitRepository habitRepository,
		IUserRepository userRepository
	)
	{
		var plan = await planRepository.GetByIdAsync(checkIn.PlanId);

        if (plan is null)
            throw new Exception(Messages.PlanNotFound);

        var habit = await habitRepository.GetByIdAsync(plan.HabitId);

        if (habit is null)
            throw new Exception(Messages.HabitNotFound);

        var user = await userRepository.GetByIdAsync(checkIn.UserId);

        if (user is null)
            throw new Exception(Messages.UserNotFound);

		return checkIn.ToModel(plan, habit, user);
	}
}
