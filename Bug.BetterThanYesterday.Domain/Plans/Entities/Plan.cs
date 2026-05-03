using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Domain.Plans.Entities;

public sealed class Plan : Entity
{
	public Guid OwnerId { get; set; }
	public Guid HabitId { get; set; }
	public string? Description { get; set; }
	public DateOnly StartsAt { get; set; }
	public DateOnly EndsAt { get; set; }
	public PlanType Type { get; set; }
	public int DaysOffPerWeek { get; set; }
	public decimal PenaltyValue { get; set; }
	public bool IsCancelled { get; set; }

	private Dictionary<PlanStatus, List<PlanStatus>> _allowedStatusChanges = new()
	{
		{ PlanStatus.NotStarted, new List<PlanStatus> { PlanStatus.Running, PlanStatus.Cancelled } },
		{ PlanStatus.Running, new List<PlanStatus> { PlanStatus.Finished, PlanStatus.Cancelled } },
		{ PlanStatus.Finished, new List<PlanStatus>() },
		{ PlanStatus.Cancelled, new List<PlanStatus>() }
	};

	private Plan(
		Guid id,
		Guid ownerId,
		Guid habitId,
		string? description,
		DateTime startsAt,
		DateTime endsAt,
		string typeIdOrName,
		int daysOffPerWeek,
		decimal penaltyValue,
		bool isCancelled,
		DateTime createdAt)
	{
		Id = id;
		OwnerId = ownerId;
		HabitId = habitId;
		Description = description;
		StartsAt = DateOnly.FromDateTime(startsAt);
		EndsAt = DateOnly.FromDateTime(endsAt);
		Type = PlanType.Get(typeIdOrName);
		DaysOffPerWeek = daysOffPerWeek;
		PenaltyValue = penaltyValue;
		IsCancelled = isCancelled;
		CreatedAt = createdAt;
	}

	private Plan(
		Guid ownerId,
		Guid habitId,
		string? description,
		DateTime startsAt,
		DateTime endsAt,
		string typeIdOrName,
		int daysOffPerWeek,
		decimal penaltyValue)
	: this(
		id: Guid.NewGuid(),
		ownerId,
		habitId,
		description,
		startsAt,
		endsAt,
		typeIdOrName,
		daysOffPerWeek,
		penaltyValue,
		isCancelled: false,
		createdAt: DateTime.Today)
	{
	}

	public static Plan CreateNew(
		Guid ownerId,
		Guid habitId,
		string? description,
		DateTime startsAt,
		DateTime endsAt,
		string typeIdOrName,
		int daysOffPerWeek,
		decimal penaltyValue)
	{
		if (ownerId == Guid.Empty)
			throw new ArgumentNullException(nameof(ownerId), Messages.EnterOwnerId);

		if (habitId == Guid.Empty)
			throw new ArgumentNullException(nameof(habitId), Messages.EnterHabitId);

		if (startsAt < DateTime.Today)
			throw new ArgumentException(Messages.StartDateCannotBeEarlierThanToday, nameof(startsAt));

		if (endsAt <= startsAt)
			throw new ArgumentException(Messages.EndDateMustBeLaterThanStartDate, nameof(endsAt));

		if (string.IsNullOrEmpty(typeIdOrName))
			throw new ArgumentException(Messages.EnterPlanType, "type");

		if (daysOffPerWeek < 0 || daysOffPerWeek > 6)
			throw new ArgumentException(Messages.EnterValidDaysOffPerWeek, nameof(daysOffPerWeek));

		if (penaltyValue < 0)
			throw new ArgumentException(Messages.EnterValidPenaltyValue, nameof(penaltyValue));

		return new Plan(
			ownerId,
			habitId,
			description,
			startsAt,
			endsAt,
			typeIdOrName,
			daysOffPerWeek,
			penaltyValue);
	}

	public static Plan Restore(
		Guid id,
		Guid ownerId,
		Guid habitId,
		string? description,
		DateTime startsAt,
		DateTime endsAt,
		string typeIdOrName,
		int daysOffPerWeek,
		decimal penaltyValue,
		bool isCancelled,
		DateTime createdAt)
	{
		if (id == Guid.Empty)
			throw new ArgumentNullException(nameof(id), Messages.EnterPlanId);

		if (ownerId == Guid.Empty)
			throw new ArgumentNullException(nameof(ownerId), Messages.EnterOwnerId);

		if (habitId == Guid.Empty)
			throw new ArgumentNullException(nameof(habitId), Messages.EnterHabitId);

		if (startsAt <= DateTime.MinValue)
			throw new ArgumentException(Messages.EnterPlanStartDate, nameof(startsAt));

		if (endsAt <= startsAt)
			throw new ArgumentException(Messages.EndDateMustBeLaterThanStartDate, nameof(endsAt));

		if (string.IsNullOrEmpty(typeIdOrName))
			throw new ArgumentException(Messages.EnterPlanType, "type");

		if (daysOffPerWeek < 0 || daysOffPerWeek > 6)
			throw new ArgumentException(Messages.EnterValidDaysOffPerWeek, nameof(daysOffPerWeek));

		if (penaltyValue < 0)
			throw new ArgumentException(Messages.EnterValidPenaltyValue, nameof(penaltyValue));

		if (createdAt == DateTime.MinValue)
			throw new ArgumentNullException(nameof(createdAt), Messages.EnterPlanCreationDate);

		return new Plan(
			id,
			ownerId,
			habitId,
			description,
			startsAt,
			endsAt,
			typeIdOrName,
			daysOffPerWeek,
			penaltyValue,
			isCancelled,
			createdAt);
	}

	public void Cancel()
	{
		IsCancelled = true;
	}

	public int GetMaxCheckInsPerDateAllowed()
	{
		// TODO implementar lógica para diferentes tipos de planos
		return 1;
	}

	public PlanStatus GetStatus()
	{
		if (IsCancelled)
			return PlanStatus.Cancelled;

		var today = DateOnly.FromDateTime(DateTime.Today);

		if (EndsAt < today)
			return PlanStatus.Finished;

		if (StartsAt > today)
			return PlanStatus.NotStarted;

		return PlanStatus.Running;
	}
}
