using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.PlanMembers.Utils;
using Bug.BetterThanYesterday.Domain.PlanMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Domain.PlanMembers.Entities;

public class PlanMember : Entity
{
	public Guid PlanId { get; set; }
	public Guid UserId { get; set; }
	public DateOnly JoinedAt { get; set; }
	public PlanMemberStatus Status { get; set; }

	private PlanMember(
		Guid id,
		Guid planId,
		Guid userId,
		DateTime joinedAt,
		string statusIdOrName,
		DateTime createdAt)
	{
		Id = id;
		PlanId = planId;
		UserId = userId;
		JoinedAt = DateOnly.FromDateTime(joinedAt);
		Status = PlanMemberStatus.Get(statusIdOrName);
		CreatedAt = createdAt;
	}

	private PlanMember(Guid planId, Guid userId)
		: this(
		id: BuildId(planId, userId),
		planId,
		userId,
		joinedAt: DateTime.Today,
		statusIdOrName: PlanMemberStatus.Active.Name,
		createdAt: DateTime.Now)
	{
	}

	public static Guid BuildId(Guid planId, Guid userId)
	{
		if (planId == Guid.Empty)
			throw new ArgumentNullException(nameof(planId), Messages.EnterPlanId);
		
		if (userId == Guid.Empty)
			throw new ArgumentNullException(nameof(userId), Messages.EnterUserId);

		return planId.Combine(userId);
	}

	public static PlanMember CreateNew(Guid planId, Guid userId)
	{
		if (planId == Guid.Empty)
			throw new ArgumentNullException(nameof(planId), Messages.EnterPlanId);
		
		if (userId == Guid.Empty)
			throw new ArgumentNullException(nameof(userId), Messages.EnterUserId);

		return new PlanMember(planId, userId);
	}

	public static PlanMember Restore(
		Guid id,
		Guid planId,
		Guid userId,
		DateTime joinedAt,
		string statusIdOrName,
		DateTime createdAt)
	{
		if (id == Guid.Empty)
			throw new ArgumentNullException(nameof(id), Messages.EnterPlanMemberId);

		if (planId == Guid.Empty)
			throw new ArgumentNullException(nameof(planId), Messages.EnterPlanId);

		if (userId == Guid.Empty)
			throw new ArgumentNullException(nameof(userId), Messages.EnterUserId);

		if (joinedAt == DateTime.MinValue)
			throw new ArgumentNullException(nameof(joinedAt), Messages.EnterPlanMemberJoinedDate);

		if (string.IsNullOrEmpty(statusIdOrName))
			throw new ArgumentException(Messages.PlanMemberStatusIdMustBeGreaterThanZero, nameof(statusIdOrName));

		if (createdAt == DateTime.MinValue)
			throw new ArgumentNullException(nameof(createdAt), Messages.EnterPlanMemberCreateDate);

		return new PlanMember(
			id,
			planId,
			userId,
			joinedAt,
			statusIdOrName,
			createdAt);
	}

	public void MarkAsBlocked()
	{
		if (Status == PlanMemberStatus.Active)
		{
			Status = PlanMemberStatus.Blocked;
			return;
		}

        if (Status == PlanMemberStatus.Blocked)
			throw new InvalidOperationException(Messages.MemberAlreadyBlockedInThisPlan);
			
		throw new InvalidOperationException(Messages.ChangeNotMappedToCurrentStatus);
    }

	public void MarkAsActive()
	{
		if (Status == PlanMemberStatus.Blocked)
		{
			Status = PlanMemberStatus.Active;
			return;
		}

        if (Status == PlanMemberStatus.Active)
			throw new InvalidOperationException(Messages.MemberIsAlreadyActiveInThePlan);
			
		throw new InvalidOperationException(Messages.ChangeNotMappedToCurrentStatus);
    }
}
