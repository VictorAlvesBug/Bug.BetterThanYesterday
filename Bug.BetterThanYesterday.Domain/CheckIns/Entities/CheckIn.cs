using System.Security.Cryptography;
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;
using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Domain.CheckIns.Entities;

public class CheckIn : Entity
{
	public Guid PlanId { get; set; }
	public Guid UserId { get; set; }
	public DateOnly Date { get; set; }
	public int Index { get; set; }
	public string Title { get; set; }
	public string PhotoUrl { get; set; }
	public CheckInStatus Status { get; set; }
	public List<Review> Reviews { get; set; }

	private CheckIn(
		Guid id,
		Guid planId,
		Guid userId,
		DateTime date,
		int index,
		string title,
		string photoUrl,
		string status,
		List<Review> reviews,
		DateTime createdAt)
	{
		Id = id;
		PlanId = planId;
		UserId = userId;
		Date = DateOnly.FromDateTime(date);
		Index = index;
		Title = title;
		PhotoUrl = photoUrl;
		Status = CheckInStatus.Get(status);
		Reviews = reviews;
		CreatedAt = createdAt;
	}

	private CheckIn(
		Guid planId,
		Guid userId,
		DateTime date,
		int index,
		string title,
		string photoUrl)
	: this(
			id: BuildId(planId, userId, date, index),
			planId,
			userId,
			date,
			index,
			title,
			photoUrl,
			status: CheckInStatus.Pending.Name,
			reviews: [],
			createdAt: DateTime.Now)
	{ }

	public static CheckIn CreateNew(
		Guid planId,
		Guid userId,
		DateTime date,
		int index,
		string title,
		string photoUrl)
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

		if (string.IsNullOrWhiteSpace(photoUrl))
			throw new ArgumentNullException(nameof(photoUrl), Messages.EnterCheckInPhotoUrl);

		return new CheckIn(planId, userId, date, index, title, photoUrl);
	}

	public static CheckIn Restore(
		Guid id,
		Guid planId,
		Guid userId,
		DateTime date,
		int index,
		string title,
		string photoUrl,
		string status,
		List<Review> reviews,
		DateTime createdAt)
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

		if (string.IsNullOrWhiteSpace(photoUrl))
			throw new ArgumentNullException(nameof(photoUrl), Messages.EnterCheckInPhotoUrl);

		if (string.IsNullOrWhiteSpace(status))
			throw new ArgumentNullException(nameof(status), Messages.EnterCheckInStatus);

		if (createdAt == DateTime.MinValue)
			throw new ArgumentNullException(nameof(createdAt), Messages.EnterCheckInCreateDate);

		return new CheckIn(
			id,
			planId,
			userId,
			date,
			index,
			title,
			photoUrl,
			status,
			reviews,
			createdAt);
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

		//TODO Remover quando terminar de testar via MoveInTime
		return Guid.NewGuid();

		//using var sha = SHA256.Create();
		//var bytes = new List<byte>();
		//bytes.AddRange(planId.ToByteArray());
		//bytes.AddRange(userId.ToByteArray());
		//bytes.AddRange(BitConverter.GetBytes(date.Date.ToBinary()));
		//bytes.AddRange(BitConverter.GetBytes(index));
		//var hash = sha.ComputeHash(bytes.ToArray());
		//return new Guid(hash.Take(16).ToArray());
	}

	public void AddReview(Review review)
	{
		Reviews.Add(review);
		Reviews = Reviews.DistinctBy(review => review.ReviewerId).ToList();
	}

	public void RemoveReviewByReviewerId(Guid reviewerId)
	{
		Reviews.RemoveAll(review => review.ReviewerId == reviewerId);
	}

	public CheckInStatus ResolveStatus(int planMemberCount, int reviewWindowInDays)
	{
		if (Status == CheckInStatus.Validated || Status == CheckInStatus.Rejected)
			return Status;

		var rejections = Reviews.Count(review => review.Status == CheckInStatus.Rejected);
		var validations = Reviews.Count(review => review.Status == CheckInStatus.Validated);

		if (rejections > planMemberCount / 2 || (Reviews.Count > 0 && rejections > Reviews.Count / 2))
			return CheckInStatus.Rejected;

		if (!IsReviewWindowOpen(reviewWindowInDays))
			return CheckInStatus.Validated;

		if (validations > rejections && validations > 0)
			return CheckInStatus.Validated;

		return CheckInStatus.Pending;
	}

	public void ConsolidateReviewsIntoStatus(int planMemberCount, int reviewWindowInDays)
	{
		Status = ResolveStatus(planMemberCount, reviewWindowInDays);
	}

	public bool IsReviewWindowOpen(int reviewWindowInDays)
	{
		return DateTime.Now <= Date.ToDateTime(TimeOnly.MinValue).AddDays(reviewWindowInDays);
	}

	public bool IsReviewAlreadyMadeByUser(Guid userId)
	{
		return Reviews.Any(review => review.ReviewerId == userId);
	}

	public bool IsReviewerCheckInOwner(Guid reviewerId)
	{
		return reviewerId == UserId;
	}
}
