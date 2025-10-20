using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.PlanParticipants.Utils;
using Bug.BetterThanYesterday.Domain.PlanParticipants.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;

public class PlanParticipant : Entity
{
	public Guid PlanId { get; set; }
	public Guid UserId { get; set; }
	public DateOnly JoinedAt { get; set; }
	public DateOnly? LeftAt { get; set; }
	public PlanParticipantStatus Status { get; set; }

	private PlanParticipant(
		Guid id,
		Guid planId,
		Guid userId,
		DateTime joinedAt,
		DateTime? leftAt,
		int statusId)
	{
		Id = id;
		PlanId = planId;
		UserId = userId;
		JoinedAt = DateOnly.FromDateTime(joinedAt);
		LeftAt = leftAt is null ? null : DateOnly.FromDateTime(leftAt.Value);
		Status = PlanParticipantStatus.FromId(statusId);
	}

	private PlanParticipant(Guid planId, Guid userId)
		: this(
		id: BuildId(planId, userId),
		planId,
		userId,
		joinedAt: DateTime.Today,
		leftAt: null,
		statusId: PlanParticipantStatus.Active.Id)
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

	public static PlanParticipant CreateNew(Guid planId, Guid userId)
	{
		if (planId == Guid.Empty)
			throw new ArgumentNullException(nameof(planId), Messages.EnterPlanId);
		
		if (userId == Guid.Empty)
			throw new ArgumentNullException(nameof(userId), Messages.EnterUserId);

		return new PlanParticipant(planId, userId);
	}

	public static PlanParticipant Restore(
		Guid id,
		Guid planId,
		Guid userId,
		DateTime joinedAt,
		DateTime? leftAt,
		int statusId)
	{
		if (id == Guid.Empty)
			throw new ArgumentNullException(nameof(id), Messages.EnterPlanParticipantId);

		if (planId == Guid.Empty)
			throw new ArgumentNullException(nameof(planId), Messages.EnterPlanId);

		if (userId == Guid.Empty)
			throw new ArgumentNullException(nameof(userId), Messages.EnterUserId);

		if (joinedAt == DateTime.MinValue)
			throw new ArgumentNullException(nameof(joinedAt), Messages.EnterPlanParticipantJoinedDate);

		if (statusId <= 0)
			throw new ArgumentException(Messages.PlanParticipantStatusIdMustBeGreaterThanZero, nameof(statusId));

		return new PlanParticipant(
			id,
			planId,
			userId,
			joinedAt,
			leftAt,
			statusId);
	}

	public void MarkAsLeft()
	{
		if (Status == PlanParticipantStatus.Active)
		{
			Status = PlanParticipantStatus.Left;
			LeftAt = DateOnly.FromDateTime(DateTime.Today);
			return;
		}

		if (Status == PlanParticipantStatus.Left)
			throw new InvalidOperationException(Messages.UserIsNotInThePlanAnymore);

		if (Status == PlanParticipantStatus.Blocked)
			throw new InvalidOperationException(Messages.UserCannotBeRemovedFromThePlanAsHeIsBlocked);
			
		throw new InvalidOperationException(Messages.ChangeNotMappedToCurrentStatus);
    }

	public void MarkAsBlocked()
	{
		if (Status == PlanParticipantStatus.Active)
		{
			Status = PlanParticipantStatus.Blocked;
			return;
		}

        if (Status == PlanParticipantStatus.Blocked)
			throw new InvalidOperationException(Messages.ParticipantAlreadyBlockedInThisPlan);

		if (Status == PlanParticipantStatus.Left)
			throw new InvalidOperationException(Messages.UserIsNotInThePlanAnymore);
			
		throw new InvalidOperationException(Messages.ChangeNotMappedToCurrentStatus);
    }

	public void MarkAsActive()
	{
		if (Status == PlanParticipantStatus.Blocked)
		{
			Status = PlanParticipantStatus.Active;
			return;
		}

        if (Status == PlanParticipantStatus.Active)
			throw new InvalidOperationException(Messages.UserIsAlreadyActive);

		if (Status == PlanParticipantStatus.Left)
			throw new InvalidOperationException(Messages.UserIsNotInThePlanAnymore);
			
		throw new InvalidOperationException(Messages.ChangeNotMappedToCurrentStatus);
    }
}
