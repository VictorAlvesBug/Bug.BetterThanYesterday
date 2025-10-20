using System.Security.Cryptography;
using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Domain.CheckIns.Entities;

public class CheckIn : Entity
{
	public Guid PlanId { get; set; }
	public Guid UserId { get; set; }
	public DateOnly Date { get; set; }
	public int Index { get; set; }
	public string Title { get; set; }
	public string? Description { get; set; }

	private CheckIn(
		Guid id,
		Guid planId,
		Guid userId,
		DateTime date,
		int index,
		string title,
		string? description)
	{
		Id = id;
		PlanId = planId;
		UserId = userId;
		Date = DateOnly.FromDateTime(date);
		Index = index;
		Title = title;
		Description = description;
	}

	private CheckIn(
		Guid planId,
		Guid userId,
		DateTime date,
		int index,
		string title,
		string? description)
	: this(
			id: BuildId(planId, userId, date, index),
			planId,
			userId,
			date,
			index,
			title,
			description)
	{ }

	public static CheckIn CreateNew(
		Guid planId,
		Guid userId,
		DateTime date,
		int index,
		string title,
		string? description)
	{
		if (planId == Guid.Empty)
			throw new ArgumentNullException(nameof(planId), Messages.EnterPlanId);

		if (userId == Guid.Empty)
			throw new ArgumentNullException(nameof(userId), Messages.EnterUserId);

		if (date == DateTime.MinValue)
			throw new ArgumentNullException(nameof(date), Messages.EnterCheckInDate);

		if (index < 0)
			throw new ArgumentNullException(nameof(index), Messages.EnterCheckInIndex);

		if (string.IsNullOrWhiteSpace(title))
			throw new ArgumentNullException(nameof(title), Messages.EnterCheckInTitle);

		return new CheckIn(planId, userId, date, index, title, description);
	}

	public static CheckIn Restore(
		Guid id,
		Guid planId,
		Guid userId,
		DateTime date,
		int index,
		string title,
		string? description)
	{
		if (id == Guid.Empty)
			throw new ArgumentNullException(nameof(id), Messages.EnterCheckInId);

		if (planId == Guid.Empty)
			throw new ArgumentNullException(nameof(planId), Messages.EnterPlanId);

		if (userId == Guid.Empty)
			throw new ArgumentNullException(nameof(userId), Messages.EnterUserId);

		if (date == DateTime.MinValue)
			throw new ArgumentNullException(nameof(date), Messages.EnterCheckInDate);

		if (index < 0)
			throw new ArgumentNullException(nameof(index), Messages.EnterCheckInIndex);

		if (string.IsNullOrWhiteSpace(title))
			throw new ArgumentNullException(nameof(title), Messages.EnterCheckInTitle);

		return new CheckIn(
			id,
			planId,
			userId,
			date,
			index,
			title,
			description);
	}

	public static Guid BuildId(Guid planId, Guid userId, DateTime date, int index = 0)
	{
		if (planId == Guid.Empty)
			throw new ArgumentNullException(nameof(planId), Messages.EnterPlanId);
		
		if (userId == Guid.Empty)
			throw new ArgumentNullException(nameof(userId), Messages.EnterUserId);

		if (date == DateTime.MinValue)
			throw new ArgumentNullException(nameof(date), Messages.EnterCheckInDate);

		if (index < 0)
			throw new ArgumentNullException(nameof(index), Messages.EnterCheckInIndex);

		using var sha = SHA256.Create();
		var bytes = new List<byte>();
		bytes.AddRange(planId.ToByteArray());
		bytes.AddRange(userId.ToByteArray());
		bytes.AddRange(BitConverter.GetBytes(date.Date.ToBinary()));
		bytes.AddRange(BitConverter.GetBytes(index));
        var hash = sha.ComputeHash(bytes.ToArray());
        return new Guid(hash.Take(16).ToArray());
	}
}
