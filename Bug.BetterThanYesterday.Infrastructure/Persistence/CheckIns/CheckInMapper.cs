using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.CheckIns;

internal sealed class CheckInMapper : IDocumentMapper<CheckIn, CheckInDocument>
{
	public CheckInDocument ToDocument(CheckIn checkIn) => new()
	{
		Id = checkIn.Id,
		PlanId = checkIn.PlanId,
		UserId = checkIn.UserId,
		Date = checkIn.Date.ToDateTime(TimeOnly.MinValue),
		Index = checkIn.Index,
		Title = checkIn.Title,
		PhotoUrl = checkIn.PhotoUrl,
		Status = checkIn.Status.Name,
		Reviews = checkIn.Reviews.Select(review => new ReviewObject()
		{
			ReviewerId = review.ReviewerId,
			Status = review.Status.Name,
			Date = review.Date
		}).ToArray(),
		CreatedAt = checkIn.CreatedAt
	};

	public CheckIn ToDomain(CheckInDocument document)
	{
		var reviews = document.Reviews
			.Select(review =>
				Review.Create(
					review.ReviewerId,
					review.Status,
					review.Date))
			.ToList();

		return CheckIn.Restore(
		document.Id,
		document.PlanId,
		document.UserId,
		document.Date,
		document.Index,
		document.Title,
		document.PhotoUrl,
		document.Status,
		reviews,
		document.CreatedAt);
	}
}
