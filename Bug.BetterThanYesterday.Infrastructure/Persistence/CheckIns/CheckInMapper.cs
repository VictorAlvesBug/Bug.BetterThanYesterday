using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.CheckIns;

internal sealed class CheckInMapper : IDocumentMapper<CheckIn, CheckInDocument>
{
	public CheckInDocument ToDocument(CheckIn checkIn) => new()
	{
		Id = checkIn.Id,
		ChallengeId = checkIn.ChallengeId,
		UserId = checkIn.UserId,
		Date = checkIn.Date,
		Index = checkIn.Index,
		Title = checkIn.Title,
		PhotoUrl = checkIn.PhotoUrl,
		Status = checkIn.Status.Name,
		Reviews = checkIn.Reviews.Select(review => new ReviewObject()
		{
			ReviewerId = review.ReviewerId,
			Status = review.Status.Name,
			Date = review.Date,
			Enable = review.Enable
		}).ToArray(),
		ReviewClosesAt = checkIn.ReviewClosesAt,
		IsReplacement = checkIn.IsReplacement,
		CreatedAt = checkIn.CreatedAt
	};

	public CheckIn ToDomain(CheckInDocument document)
	{
		var reviews = document.Reviews
			.Select(review =>
			{
				var restored = Review.Create(
					review.ReviewerId,
					review.Status,
					review.Date);
				return review.Enable ? restored : restored.Disable();
			})
			.ToList();

		return CheckIn.Restore(
		document.Id,
		document.ChallengeId,
		document.UserId,
		document.Date,
		document.Index,
		document.Title,
		document.PhotoUrl,
		document.Status,
		reviews,
		document.CreatedAt,
		document.ReviewClosesAt,
		document.IsReplacement);
	}
}
