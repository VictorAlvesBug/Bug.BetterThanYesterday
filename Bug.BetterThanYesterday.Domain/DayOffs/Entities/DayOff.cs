using System.Security.Cryptography;
using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Domain.DayOffs.Entities;

public sealed class DayOff : Entity
{
	public Guid PlanId { get; set; }
	public Guid UserId { get; set; }
	public DateOnly Date { get; set; }

	private DayOff(
		Guid id,
		Guid planId,
		Guid userId,
		DateTime date,
		DateTime createdAt)
	{
		Id = id;
		PlanId = planId;
		UserId = userId;
		Date = DateOnly.FromDateTime(date);
		CreatedAt = createdAt;
	}

	private DayOff(Guid planId, Guid userId, DateTime date)
		: this(
			id: BuildId(planId, userId, date),
			planId,
			userId,
			date,
			createdAt: DateTime.Now)
	{
	}

	public static DayOff CreateNew(Guid planId, Guid userId, DateTime date)
	{
		if (planId == Guid.Empty)
			throw new ArgumentNullException(nameof(planId), Messages.EnterPlanId);

		if (userId == Guid.Empty)
			throw new ArgumentNullException(nameof(userId), Messages.EnterUserId);

		if (date == DateTime.MinValue)
			throw new ArgumentNullException(nameof(date), Messages.EnterDayOffDate);

		return new DayOff(planId, userId, date);
	}

	public static DayOff Restore(
		Guid id,
		Guid planId,
		Guid userId,
		DateTime date,
		DateTime createdAt)
	{
		if (id == Guid.Empty)
			throw new ArgumentNullException(nameof(id), Messages.EnterDayOffId);

		if (planId == Guid.Empty)
			throw new ArgumentNullException(nameof(planId), Messages.EnterPlanId);

		if (userId == Guid.Empty)
			throw new ArgumentNullException(nameof(userId), Messages.EnterUserId);

		if (date == DateTime.MinValue)
			throw new ArgumentNullException(nameof(date), Messages.EnterDayOffDate);

		if (createdAt == DateTime.MinValue)
			throw new ArgumentNullException(nameof(createdAt), Messages.EnterDayOffCreateDate);

		return new DayOff(id, planId, userId, date, createdAt);
	}

	public static Guid BuildId(Guid planId, Guid userId, DateTime date)
	{
		if (planId == Guid.Empty)
			throw new ArgumentNullException(nameof(planId), Messages.EnterPlanId);

		if (userId == Guid.Empty)
			throw new ArgumentNullException(nameof(userId), Messages.EnterUserId);

		if (date == DateTime.MinValue)
			throw new ArgumentNullException(nameof(date), Messages.EnterDayOffDate);

		//TODO Remover quando terminar de testar via MoveInTime
		return Guid.NewGuid();
			
        //using var sha = SHA256.Create();
		//var bytes = new List<byte> { 0xD0, 0xFF };
		//bytes.AddRange(planId.ToByteArray());
		//bytes.AddRange(userId.ToByteArray());
		//bytes.AddRange(BitConverter.GetBytes(date.Date.ToBinary()));
		//var hash = sha.ComputeHash(bytes.ToArray());
		//return new Guid(hash.Take(16).ToArray());
	}
}
