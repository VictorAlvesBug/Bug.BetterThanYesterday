using Bug.BetterThanYesterday.Application.Penalties;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.Notifications;
using Bug.BetterThanYesterday.Domain.Notifications.Entities;
using Bug.BetterThanYesterday.Domain.Notifications.ValueObjects;
using Bug.BetterThanYesterday.Domain.Penalties;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;
using Bug.BetterThanYesterday.Domain.PlanMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;

namespace Bug.BetterThanYesterday.Application.Notifications;

public interface IPlanCycleNotificationProcessor
{
	Task NotifyCheckInAsync(Guid planId, Guid authorId, string authorName, IReadOnlyList<PlanMember> members);
	Task NotifyPlanCancelledForQuorumAsync(Guid planId, IReadOnlyList<Guid> userIds);
	Task GenerateForUserAsync(Guid userId);
}

public sealed class PlanCycleNotificationProcessor(
	IUserNotificationRepository notificationRepository,
	IPlanRepository planRepository,
	IPlanMemberRepository planMemberRepository,
	ICheckInRepository checkInRepository,
	IPenaltyRepository penaltyRepository)
	: IPlanCycleNotificationProcessor
{
	public async Task NotifyCheckInAsync(Guid planId, Guid authorId, string authorName, IReadOnlyList<PlanMember> members)
	{
		var today = DateOnly.FromDateTime(DateTime.Today);
		var message = $"{authorName} fez o check-in";

		foreach (var member in members.Where(member => member.UserId != authorId))
			await AddOnceAsync(member.UserId, planId, NotificationKind.CheckInMade, message, today, authorId);
	}

	public async Task NotifyPlanCancelledForQuorumAsync(Guid planId, IReadOnlyList<Guid> userIds)
	{
		var today = DateOnly.FromDateTime(DateTime.Today);
		const string message = "O plano foi cancelado porque restou menos de dois membros";

		foreach (var userId in userIds.Distinct())
			await AddOnceAsync(userId, planId, NotificationKind.PlanCancelledForQuorum, message, today, null);
	}

	public async Task GenerateForUserAsync(Guid userId)
	{
		var memberships = await planMemberRepository.ListByUserIdAsync(userId);
		var today = DateOnly.FromDateTime(DateTime.Today);

		foreach (var membership in memberships)
		{
			var plan = await planRepository.GetByIdAsync(membership.PlanId);

			if (plan is null || plan.GetStatus() == PlanStatus.Cancelled)
				continue;

			if (plan.StartsAt == today)
			{
				await AddOnceAsync(
					userId,
					plan.Id,
					NotificationKind.PlanStartsToday,
					"O plano começa hoje",
					today,
					null);
			}

			if (plan.GetStatus() != PlanStatus.Running || membership.Status != PlanMemberStatus.Active)
				continue;

			var checkIns = await checkInRepository.ListByPlanIdAndUserIdAsync(plan.Id, userId);
			var coverage = DayOffAvailabilityCalculator.EvaluateDayCoverage(today, checkIns);

			if (coverage != DayCoverage.Missed)
				continue;

			var penalties = await penaltyRepository.ListByPlanIdAndUserIdAsync(plan.Id, userId);
			var wouldUseRest = DayOffAvailabilityCalculator.IsAutoRestDay(today, membership.JoinedAt, plan, checkIns, penalties);
			var kind = wouldUseRest ? NotificationKind.RestAtRisk : NotificationKind.FineAtRisk;
			var message = wouldUseRest
				? "Bora hoje, ou prefere tirar uma folga?"
				: "Folgas esgotadas! Faça check-in para não levar uma multa!";

			await AddOnceAsync(userId, plan.Id, kind, message, today, null);
		}
	}

	private async Task AddOnceAsync(
		Guid userId,
		Guid planId,
		NotificationKind kind,
		string message,
		DateOnly day,
		Guid? relatedUserId)
	{
		var existing = await notificationRepository.ListByUserIdAsync(userId);

		if (existing.Any(notification =>
			notification.PlanId == planId
			&& notification.Kind == kind
			&& notification.Day == day
			&& notification.RelatedUserId == relatedUserId))
			return;

		await notificationRepository.AddAsync(UserNotification.Create(userId, planId, kind, message, day, relatedUserId));
	}
}
