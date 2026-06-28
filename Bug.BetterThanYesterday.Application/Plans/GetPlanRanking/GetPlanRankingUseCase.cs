using Bug.BetterThanYesterday.Application.DayOffs;
using Bug.BetterThanYesterday.Application.Plans.Models;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;
using Bug.BetterThanYesterday.Domain.DayOffs;
using Bug.BetterThanYesterday.Domain.DayOffs.Entities;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.PlanMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.Plans.GetPlanRanking;

public sealed class GetPlanRankingUseCase(
	IPlanRepository planRepository,
	IPlanMemberRepository planMemberRepository,
	ICheckInRepository checkInRepository,
	IDayOffRepository dayOffRepository,
	IUserRepository userRepository)
	: IUseCase<GetPlanRankingCommand>
{
	public async Task<IResult> HandleAsync(GetPlanRankingCommand command)
	{
		try
		{
			command.Validate();

			var plan = await planRepository.GetByIdAsync(command.PlanId);

			if (plan is null)
				return Result.Rejected(Messages.PlanNotFound, RejectionType.NotFound);

			var today = DateOnly.FromDateTime(DateTime.Today);
			var elapsedEnd = plan.EndsAt < today ? plan.EndsAt : today;
			var totalCheckinCount = elapsedEnd >= plan.StartsAt
				? elapsedEnd.DayNumber - plan.StartsAt.DayNumber + 1
				: 0;

			var members = (await planMemberRepository.ListByPlanIdAsync(command.PlanId))
				.Where(m => m.Status == PlanMemberStatus.Active)
				.ToList();

			var planCheckIns = await checkInRepository.ListByPlanIdAsync(command.PlanId);
			var planDayOffs = await dayOffRepository.ListByPlanIdAsync(command.PlanId);

			var items = new List<PlanRankingItemModel>();

			var activeMemberCount = members.Count;

			foreach (var member in members)
			{
				var user = await userRepository.GetByIdAsync(member.UserId);

				if (user is null)
					continue;

				var memberCheckIns = planCheckIns.Where(c => c.UserId == member.UserId).ToList();
				var memberDayOffs = planDayOffs.Where(d => d.UserId == member.UserId).ToList();

				var validatedDates = memberCheckIns
					.Where(c => c.ResolveStatus(activeMemberCount, plan.CheckInReviewWindowInDays) == CheckInStatus.Validated)
					.Select(c => c.Date)
					.ToHashSet();

				var validatedCount = validatedDates.Count;
				var dayOffCount = memberDayOffs.Count(d => d.Date <= elapsedEnd);
				var missedDays = Math.Max(0, totalCheckinCount - validatedCount - dayOffCount);
				var penalty = missedDays * plan.PenaltyValue;
				var streak = CalculateStreak(validatedDates, memberDayOffs, today, plan.StartsAt);

				items.Add(new PlanRankingItemModel
				{
					UserId = member.UserId,
					UserName = user.Nickname,
					CheckinCount = validatedCount,
					Penalty = penalty,
					Streak = streak
				});
			}

			items = items
				.OrderByDescending(i => i.CheckinCount)
				.ThenBy(i => i.Penalty)
				.ThenByDescending(i => i.Streak)
				.ToList();

			for (var i = 0; i < items.Count; i++)
				items[i].Position = i + 1;

			PlanRankingItemModel? currentUser = null;
			var daysOffAvailable = 0;

			if (command.UserId is not null)
			{
				currentUser = items.FirstOrDefault(i => i.UserId == command.UserId.Value);
				var userDayOffs = planDayOffs.Where(d => d.UserId == command.UserId.Value);
				daysOffAvailable = DayOffMapper.GetDaysOffAvailable(plan, userDayOffs, today);
			}

			return Result.Success(
				new PlanRankingModel
				{
					TotalCheckinCount = totalCheckinCount,
					DaysOffAvailable = daysOffAvailable,
					Items = items,
					CurrentUser = currentUser
				},
				Messages.PlanRankingSuccessfullyFound);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}

	public static int CalculateStreak(
		IReadOnlySet<DateOnly> validatedDates,
		IReadOnlyList<DayOff> dayOffs,
		DateOnly today,
		DateOnly planStartsAt)
	{
		var dayOffDates = dayOffs.Select(d => d.Date).ToHashSet();

		var streak = 0;
		var cursor = today;

		while (cursor >= planStartsAt)
		{
			if (validatedDates.Contains(cursor) || dayOffDates.Contains(cursor))
			{
				streak++;
				cursor = cursor.AddDays(-1);
				continue;
			}

			if (cursor == today)
			{
				cursor = cursor.AddDays(-1);
				continue;
			}

			break;
		}

		return streak;
	}
}
