using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;
using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Domain.CheckIns.Entities;

public class CheckIn : Entity
{
    public Guid PlanId { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public int Index { get; set; }
    public string Title { get; set; }
    public string PhotoUrl { get; set; }
    public CheckInStatus Status { get; set; }
    public List<Review> Reviews { get; set; }
    public DateTime ReviewClosesAt { get; set; }
    public bool IsReplacement { get; set; }

    public DateOnly CalendarDate => DateOnly.FromDateTime(Date);

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
        DateTime createdAt,
        DateTime? reviewClosesAt = null,
        bool isReplacement = false)
    {
        Id = id;
        PlanId = planId;
        UserId = userId;
        Date = date;
        Index = index;
        Title = title;
        PhotoUrl = photoUrl;
        Status = CheckInStatus.Get(status);
        Reviews = reviews;
        CreatedAt = createdAt;
        ReviewClosesAt = reviewClosesAt is null || reviewClosesAt == DateTime.MinValue
            ? createdAt.AddHours(24)
            : reviewClosesAt.Value;
        IsReplacement = isReplacement;
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
        DateTime createdAt,
        DateTime? reviewClosesAt = null,
        bool isReplacement = false)
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
            createdAt,
            reviewClosesAt,
            isReplacement);
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
    }

    public void AddReview(Review review)
    {
        Reviews.RemoveAll(existing => existing.ReviewerId == review.ReviewerId && !existing.Enable);
        if (Reviews.Any(existing => existing.ReviewerId == review.ReviewerId && existing.Enable))
            return;

        Reviews.Add(review);
    }

    public void RemoveReviewByReviewerId(Guid reviewerId)
    {
        Reviews.RemoveAll(review => review.ReviewerId == reviewerId);
    }

    public CheckInStatus ResolveStatus()
    {
        if (IsReviewWindowOpen())
            return CheckInStatus.Pending;

        var activeReviews = Reviews.Where(review => review.Enable).ToList();
        var rejections = activeReviews.Count(review => review.Status == CheckInStatus.Rejected);

        if (activeReviews.Count > 0 && rejections > activeReviews.Count / 2)
            return CheckInStatus.Rejected;

        return CheckInStatus.Validated;
    }

    public void ConsolidateReviewsIntoStatus()
    {
        if (IsReviewWindowOpen())
            return;

        Status = ResolveStatus();
    }

    public bool IsReviewWindowOpen()
    {
        return DateTime.Now < ReviewClosesAt;
    }

    public bool CanReplace()
    {
        return !IsReplacement && DateTime.Now <= ReviewClosesAt.AddHours(-4);
    }

    public void ReplaceEvidence(string title, string photoUrl)
    {
        if (IsReplacement)
            throw new InvalidOperationException(Messages.CheckInAlreadyReplaced);

        if (!CanReplace())
            throw new InvalidOperationException(Messages.CheckInReplacementWindowHasClosed);

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentNullException(nameof(title), Messages.EnterCheckInTitle);

        if (string.IsNullOrWhiteSpace(photoUrl))
            throw new ArgumentNullException(nameof(photoUrl), Messages.EnterCheckInPhotoUrl);

        Title = title;
        PhotoUrl = photoUrl;
        IsReplacement = true;
        Reviews = Reviews.Select(review => review.Disable()).ToList();
    }

    public bool IsReviewAlreadyMadeByUser(Guid userId)
    {
        return Reviews.Any(review => review.Enable && review.ReviewerId == userId);
    }

    public bool IsReviewerCheckInOwner(Guid reviewerId)
    {
        return reviewerId == UserId;
    }
}
