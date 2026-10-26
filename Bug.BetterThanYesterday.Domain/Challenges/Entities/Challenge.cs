using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Domain.Challenges.Entities;

public sealed class Challenge : Entity
{
	public Guid OwnerId { get; set; }
	public Guid HabitId { get; set; }
	public string? Description { get; set; }
	public DateOnly StartsAt { get; set; }
	public DateOnly EndsAt { get; set; }
	public ChallengeType Type { get; set; }
	public int DaysOffPerWeek { get; set; }
	public decimal PenaltyValue { get; set; }
	public bool IsCancelled { get; set; }
	public string? CoverPhotoUrl { get; set; }

	public int CheckInReviewWindowInDays = 1;

	private Challenge(
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
		DateTime createdAt,
		string? coverPhotoUrl = null)
	{
		Id = id;
		OwnerId = ownerId;
		HabitId = habitId;
		Description = description;
		StartsAt = DateOnly.FromDateTime(startsAt);
		EndsAt = DateOnly.FromDateTime(endsAt);
		Type = ChallengeType.Get(typeIdOrName);
		DaysOffPerWeek = daysOffPerWeek;
		PenaltyValue = penaltyValue;
		IsCancelled = isCancelled;
		CreatedAt = createdAt;
		CoverPhotoUrl = coverPhotoUrl;
	}

	private Challenge(
		Guid ownerId,
		Guid habitId,
		string? description,
		DateTime startsAt,
		DateTime endsAt,
		string typeIdOrName,
		int daysOffPerWeek,
		decimal penaltyValue,
		string? coverPhotoUrl = null)
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
		createdAt: DateTime.Now,
		coverPhotoUrl)
	{
	}

	public static Challenge CreateNew(
		Guid ownerId,
		Guid habitId,
		string? description,
		DateTime startsAt,
		DateTime endsAt,
		string typeIdOrName,
		int daysOffPerWeek,
		decimal penaltyValue,
		string? coverPhotoUrl = null)
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
			throw new ArgumentException(Messages.EnterChallengeType, "type");

		if (daysOffPerWeek < 0 || daysOffPerWeek > 6)
			throw new ArgumentException(Messages.EnterValidDaysOffPerWeek, nameof(daysOffPerWeek));

		if (penaltyValue < 0)
			throw new ArgumentException(Messages.EnterValidPenaltyValue, nameof(penaltyValue));

		return new Challenge(
			ownerId,
			habitId,
			description,
			startsAt,
			endsAt,
			typeIdOrName,
			daysOffPerWeek,
			penaltyValue,
			coverPhotoUrl);
	}

	public static Challenge Restore(
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
		DateTime createdAt,
		string? coverPhotoUrl = null)
	{
		if (id == Guid.Empty)
			throw new ArgumentNullException(nameof(id), Messages.EnterChallengeId);

		if (ownerId == Guid.Empty)
			throw new ArgumentNullException(nameof(ownerId), Messages.EnterOwnerId);

		if (habitId == Guid.Empty)
			throw new ArgumentNullException(nameof(habitId), Messages.EnterHabitId);

		if (startsAt <= DateTime.MinValue)
			throw new ArgumentException(Messages.EnterChallengeStartDate, nameof(startsAt));

		if (endsAt <= startsAt)
			throw new ArgumentException(Messages.EndDateMustBeLaterThanStartDate, nameof(endsAt));

		if (string.IsNullOrEmpty(typeIdOrName))
			throw new ArgumentException(Messages.EnterChallengeType, "type");

		if (daysOffPerWeek < 0 || daysOffPerWeek > 6)
			throw new ArgumentException(Messages.EnterValidDaysOffPerWeek, nameof(daysOffPerWeek));

		if (penaltyValue < 0)
			throw new ArgumentException(Messages.EnterValidPenaltyValue, nameof(penaltyValue));

		if (createdAt == DateTime.MinValue)
			throw new ArgumentNullException(nameof(createdAt), Messages.EnterChallengeCreationDate);

		return new Challenge(
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
			createdAt,
			coverPhotoUrl);
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

	public ChallengeStatus GetStatus()
	{
		if (IsCancelled)
			return ChallengeStatus.Cancelled;

		var today = DateOnly.FromDateTime(DateTime.Today);

		if (EndsAt < today)
			return ChallengeStatus.Finished;

		if (StartsAt > today)
			return ChallengeStatus.NotStarted;

		return ChallengeStatus.Running;
	}

	public ChallengeStatus GetStatus(int memberCount)
	{
		var status = GetStatus();

		if (status == ChallengeStatus.Running && memberCount < 2)
			return ChallengeStatus.NotStarted;

		return status;
	}

	public void UpdatePeriod(DateOnly startsAt, DateOnly endsAt, int memberCount)
	{
		if (GetStatus(memberCount) != ChallengeStatus.NotStarted)
			throw new InvalidOperationException(Messages.OnlyNotStartedChallengesCanBeUpdated);

		if (endsAt <= startsAt)
			throw new ArgumentException(Messages.EndDateMustBeLaterThanStartDate, nameof(endsAt));

		if (startsAt <= DateOnly.FromDateTime(DateTime.Today))
			throw new ArgumentException(Messages.StartDateMustBeFromTomorrow, nameof(startsAt));

		StartsAt = startsAt;
		EndsAt = endsAt;
	}
}
