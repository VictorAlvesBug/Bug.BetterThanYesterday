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
	Task NotifyCheckInAsync(Guid planId, Guid checkInId, Guid authorId, string authorName, IReadOnlyList<PlanMember> members);
	Task NotifyCheckInReviewedAsync(Guid planId, Guid checkInId, Guid authorId, Guid reviewerId, string reviewerName, bool rejected, bool atRiskOfRejection);
	Task NotifyCheckInReportedAsync(Guid planId, Guid checkInId, Guid reporterId, Guid targetUserId, string message, IReadOnlyList<Guid> recipientIds);
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
	public async Task NotifyCheckInAsync(Guid planId, Guid checkInId, Guid authorId, string authorName, IReadOnlyList<PlanMember> members)
	{
		var today = DateOnly.FromDateTime(DateTime.Today);
		var message = $"{authorName} fez check-in";
		var payload = new CheckInMadePayload { PlanId = planId, CheckInId = checkInId, AuthorId = authorId };

		foreach (var member in members.Where(member => member.UserId != authorId))
			await AddOnceAsync(member.UserId, payload, NotificationKind.CheckInMade, message, today, authorId);
	}

	public async Task NotifyCheckInReviewedAsync(
		Guid planId,
		Guid checkInId,
		Guid authorId,
		Guid reviewerId,
		string reviewerName,
		bool rejected,
		bool atRiskOfRejection)
	{
		var today = DateOnly.FromDateTime(DateTime.Today);
		var reviewMessage = rejected
			? $"{reviewerName} rejeitou seu check-in"
			: $"{reviewerName} validou seu check-in";

		await AddOnceAsync(
			authorId,
			new CheckInReviewedPayload { PlanId = planId, CheckInId = checkInId, ReviewerId = reviewerId },
			NotificationKind.CheckInReviewed,
			reviewMessage,
			today,
			reviewerId);

		if (!atRiskOfRejection)
			return;

		await AddOnceAsync(
			authorId,
			new CheckInRejectionRiskPayload { PlanId = planId, CheckInId = checkInId },
			NotificationKind.CheckInRejectionRisk,
			"Seu check-in não está sendo bem aceito. Substitua antes que seja rejeitado.",
			today,
			null);
	}

	public async Task NotifyCheckInReportedAsync(
		Guid planId,
		Guid checkInId,
		Guid reporterId,
		Guid targetUserId,
		string message,
		IReadOnlyList<Guid> recipientIds)
	{
		var today = DateOnly.FromDateTime(DateTime.Today);
		var payload = new CheckInReportedPayload
		{
			PlanId = planId,
			CheckInId = checkInId,
			ReporterId = reporterId,
			TargetUserId = targetUserId
		};

		foreach (var recipientId in recipientIds.Where(id => id != Guid.Empty).Distinct())
			await AddOnceAsync(recipientId, payload, NotificationKind.CheckInReported, message, today, reporterId);
	}

	public async Task NotifyPlanCancelledForQuorumAsync(Guid planId, IReadOnlyList<Guid> userIds)
	{
		var today = DateOnly.FromDateTime(DateTime.Today);
		const string message = "O plano foi cancelado porque restou menos de dois membros";
		var payload = new PlanCancelledForQuorumPayload { PlanId = planId };

		foreach (var userId in userIds.Distinct())
			await AddOnceAsync(userId, payload, NotificationKind.PlanCancelledForQuorum, message, today, null);
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
					new PlanStartsTodayPayload { PlanId = plan.Id },
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
				? "Bora hoje, ou prefere descansar?"
				: "Folgas esgotadas! Faça check-in para não levar uma multa!";
			NotificationPayload payload = wouldUseRest
				? new RestAtRiskPayload { PlanId = plan.Id }
				: new FineAtRiskPayload { PlanId = plan.Id };

			await AddOnceAsync(userId, payload, kind, message, today, null);
		}
	}

	private async Task AddOnceAsync(
		Guid userId,
		NotificationPayload payload,
		NotificationKind kind,
		string message,
		DateOnly day,
		Guid? relatedUserId)
	{
		var existing = await notificationRepository.ListByUserIdAsync(userId);

		if (existing.Any(notification =>
			notification.PlanId == payload.PlanId
			&& notification.Kind == kind
			&& notification.Day == day
			&& notification.RelatedUserId == relatedUserId
			&& notification.Payload.ReadCheckInId() == payload.ReadCheckInId()))
			return;

		await notificationRepository.AddAsync(UserNotification.Create(userId, payload, kind, message, day, relatedUserId));
	}
}
