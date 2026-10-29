using Bug.BetterThanYesterday.Domain.ChallengeMembers.Entities;

namespace Bug.BetterThanYesterday.Application.Notifications;

public interface IChallengeCycleNotificationProcessor
{
	Task NotifyCheckInAsync(Guid challengeId, Guid checkInId, Guid authorId, string authorName, IReadOnlyList<ChallengeMember> members);
	Task NotifyCheckInReviewedAsync(Guid challengeId, Guid checkInId, Guid authorId, Guid reviewerId, string reviewerName, bool rejected, bool atRiskOfRejection);
	Task NotifyCheckInReportedAsync(Guid challengeId, Guid checkInId, Guid reporterId, Guid targetUserId, string message, IReadOnlyList<Guid> recipientIds);
	Task NotifyChallengeCancelledForQuorumAsync(Guid challengeId, IReadOnlyList<Guid> userIds);
	Task GenerateForUserAsync(Guid userId);
}
