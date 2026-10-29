using Bug.BetterThanYesterday.Application.Penalties;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.Notifications;
using Bug.BetterThanYesterday.Domain.Notifications.Entities;
using Bug.BetterThanYesterday.Domain.Notifications.ValueObjects;
using Bug.BetterThanYesterday.Domain.Penalties;
using Bug.BetterThanYesterday.Domain.ChallengeMembers;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.Entities;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;

namespace Bug.BetterThanYesterday.Application.Notifications;

public sealed class ChallengeCycleNotificationProcessor(
	IUserNotificationRepository notificationRepository,
	IChallengeRepository challengeRepository,
	IChallengeMemberRepository challengeMemberRepository,
	ICheckInRepository checkInRepository,
	IPenaltyRepository penaltyRepository)
	: IChallengeCycleNotificationProcessor
{
	public async Task NotifyCheckInAsync(Guid challengeId, Guid checkInId, Guid authorId, string authorName, IReadOnlyList<ChallengeMember> members)
	{
		var today = DateOnly.FromDateTime(DateTime.Today);
		var message = $"{authorName} marcou presença";
		var payload = new CheckInMadePayload { ChallengeId = challengeId, CheckInId = checkInId, AuthorId = authorId };

		foreach (var member in members.Where(member => member.UserId != authorId))
			await AddOnceAsync(member.UserId, payload, NotificationKind.CheckInMade, message, today, authorId);
	}

	public async Task NotifyCheckInReviewedAsync(
		Guid challengeId,
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
			: $"{reviewerName} validou seu check-in. Ponto na conta!";

		await AddOnceAsync(
			authorId,
			new CheckInReviewedPayload { ChallengeId = challengeId, CheckInId = checkInId, ReviewerId = reviewerId },
			NotificationKind.CheckInReviewed,
			reviewMessage,
			today,
			reviewerId);

		if (!atRiskOfRejection)
			return;

		await AddOnceAsync(
			authorId,
			new CheckInRejectionRiskPayload { ChallengeId = challengeId, CheckInId = checkInId },
			NotificationKind.CheckInRejectionRisk,
			"Seu check-in está perdendo a votação. Troca ele antes que vire rejeição.",
			today,
			null);
	}

	public async Task NotifyCheckInReportedAsync(
		Guid challengeId,
		Guid checkInId,
		Guid reporterId,
		Guid targetUserId,
		string message,
		IReadOnlyList<Guid> recipientIds)
	{
		var today = DateOnly.FromDateTime(DateTime.Today);
		var payload = new CheckInReportedPayload
		{
			ChallengeId = challengeId,
			CheckInId = checkInId,
			ReporterId = reporterId,
			TargetUserId = targetUserId
		};

		foreach (var recipientId in recipientIds.Where(id => id != Guid.Empty).Distinct())
			await AddOnceAsync(recipientId, payload, NotificationKind.CheckInReported, message, today, reporterId);
	}

	public async Task NotifyChallengeCancelledForQuorumAsync(Guid challengeId, IReadOnlyList<Guid> userIds)
	{
		var today = DateOnly.FromDateTime(DateTime.Today);
		const string message = "O desafio foi cancelado porque ficou com menos de dois jogadores";
		var payload = new ChallengeCancelledForQuorumPayload { ChallengeId = challengeId };

		foreach (var userId in userIds.Distinct())
			await AddOnceAsync(userId, payload, NotificationKind.ChallengeCancelledForQuorum, message, today, null);
	}

	public async Task GenerateForUserAsync(Guid userId)
	{
		var memberships = await challengeMemberRepository.ListByUserIdAsync(userId);
		var today = DateOnly.FromDateTime(DateTime.Today);

		foreach (var membership in memberships)
		{
			var challenge = await challengeRepository.GetByIdAsync(membership.ChallengeId);

			if (challenge is null || challenge.GetStatus() == ChallengeStatus.Cancelled)
				continue;

			if (challenge.StartsAt == today)
			{
				await AddOnceAsync(
					userId,
					new ChallengeStartsTodayPayload { ChallengeId = challenge.Id },
					NotificationKind.ChallengeStartsToday,
					"O desafio começa hoje. Entra em campo!",
					today,
					null);
			}

			if (challenge.GetStatus() != ChallengeStatus.Running || membership.Status != ChallengeMemberStatus.Active)
				continue;

			var checkIns = await checkInRepository.ListByChallengeIdAndUserIdAsync(challenge.Id, userId);
			var coverage = DayOffAvailabilityCalculator.EvaluateDayCoverage(today, checkIns);

			if (coverage != DayCoverage.Missed)
				continue;

			var penalties = await penaltyRepository.ListByChallengeIdAndUserIdAsync(challenge.Id, userId);
			var wouldUseRest = DayOffAvailabilityCalculator.IsAutoRestDay(today, membership.JoinedAt, challenge, checkIns, penalties);
			var kind = wouldUseRest ? NotificationKind.RestAtRisk : NotificationKind.FineAtRisk;
			var message = wouldUseRest
				? "Bora hoje, ou prefere descansar?"
				: "Folgas zeradas! Marca o check-in ou a multa entra em campo.";
			NotificationPayload payload = wouldUseRest
				? new RestAtRiskPayload { ChallengeId = challenge.Id }
				: new FineAtRiskPayload { ChallengeId = challenge.Id };

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
			notification.ChallengeId == payload.ChallengeId
			&& notification.Kind == kind
			&& notification.Day == day
			&& notification.RelatedUserId == relatedUserId
			&& notification.Payload.ReadCheckInId() == payload.ReadCheckInId()))
			return;

		await notificationRepository.AddAsync(UserNotification.Create(userId, payload, kind, message, day, relatedUserId));
	}
}
