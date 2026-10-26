using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.Utils;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Domain.ChallengeMembers.Entities;

public class ChallengeMember : Entity
{
	public Guid ChallengeId { get; set; }
	public Guid UserId { get; set; }
	public DateOnly JoinedAt { get; set; }
	public ChallengeMemberStatus Status { get; set; }
	public bool IsDelinquent { get; private set; }

	private ChallengeMember(
		Guid id,
		Guid challengeId,
		Guid userId,
		DateTime joinedAt,
		string statusIdOrName,
		DateTime createdAt,
		bool isDelinquent = false)
	{
		Id = id;
		ChallengeId = challengeId;
		UserId = userId;
		JoinedAt = DateOnly.FromDateTime(joinedAt);
		Status = ChallengeMemberStatus.Get(statusIdOrName);
		CreatedAt = createdAt;
		IsDelinquent = isDelinquent;
	}

	private ChallengeMember(Guid challengeId, Guid userId)
		: this(
		id: BuildId(challengeId, userId),
		challengeId,
		userId,
		joinedAt: DateTime.Today,
		statusIdOrName: ChallengeMemberStatus.Active.Name,
		createdAt: DateTime.Now)
	{
	}

	public static Guid BuildId(Guid challengeId, Guid userId)
	{
		if (challengeId == Guid.Empty)
			throw new ArgumentNullException(nameof(challengeId), Messages.EnterChallengeId);
		
		if (userId == Guid.Empty)
			throw new ArgumentNullException(nameof(userId), Messages.EnterUserId);

		return challengeId.Combine(userId);
	}

	public static ChallengeMember CreateNew(Guid challengeId, Guid userId)
	{
		if (challengeId == Guid.Empty)
			throw new ArgumentNullException(nameof(challengeId), Messages.EnterChallengeId);
		
		if (userId == Guid.Empty)
			throw new ArgumentNullException(nameof(userId), Messages.EnterUserId);

		return new ChallengeMember(challengeId, userId);
	}

	public static ChallengeMember Restore(
		Guid id,
		Guid challengeId,
		Guid userId,
		DateTime joinedAt,
		string statusIdOrName,
		DateTime createdAt,
		bool isDelinquent = false)
	{
		if (id == Guid.Empty)
			throw new ArgumentNullException(nameof(id), Messages.EnterChallengeMemberId);

		if (challengeId == Guid.Empty)
			throw new ArgumentNullException(nameof(challengeId), Messages.EnterChallengeId);

		if (userId == Guid.Empty)
			throw new ArgumentNullException(nameof(userId), Messages.EnterUserId);

		if (joinedAt == DateTime.MinValue)
			throw new ArgumentNullException(nameof(joinedAt), Messages.EnterChallengeMemberJoinedDate);

		if (string.IsNullOrEmpty(statusIdOrName))
			throw new ArgumentException(Messages.ChallengeMemberStatusIdMustBeGreaterThanZero, nameof(statusIdOrName));

		if (createdAt == DateTime.MinValue)
			throw new ArgumentNullException(nameof(createdAt), Messages.EnterChallengeMemberCreateDate);

		return new ChallengeMember(
			id,
			challengeId,
			userId,
			joinedAt,
			statusIdOrName,
			createdAt,
			isDelinquent);
	}

	public void MarkAsDelinquent()
	{
		IsDelinquent = true;
	}

	public void MarkAsBlocked()
	{
		if (Status == ChallengeMemberStatus.Active)
		{
			Status = ChallengeMemberStatus.Blocked;
			return;
		}

        if (Status == ChallengeMemberStatus.Blocked)
			throw new InvalidOperationException(Messages.MemberAlreadyBlockedInThisChallenge);
			
		throw new InvalidOperationException(Messages.ChangeNotMappedToCurrentStatus);
    }

	public void MarkAsActive()
	{
		if (Status == ChallengeMemberStatus.Blocked)
		{
			Status = ChallengeMemberStatus.Active;
			return;
		}

        if (Status == ChallengeMemberStatus.Active)
			throw new InvalidOperationException(Messages.MemberIsAlreadyActiveInTheChallenge);
			
		throw new InvalidOperationException(Messages.ChangeNotMappedToCurrentStatus);
    }
}
