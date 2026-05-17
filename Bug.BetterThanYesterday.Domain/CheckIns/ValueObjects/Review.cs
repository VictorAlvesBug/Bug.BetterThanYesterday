using Bug.BetterThanYesterday.Domain.Extensions;
using Bug.BetterThanYesterday.Domain.Strings;
using System.Text.RegularExpressions;

namespace Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;

public sealed record Review
{
	public Guid ReviewerId { get; }
	public CheckInStatus Status { get; }
	public DateTime Date { get; }

	private Review(
		Guid reviewerId,
		string status,
		DateTime date)
	{
		ReviewerId = reviewerId;
		Status = CheckInStatus.Get(status);
		Date = date;

		if (!Status.IsValidAsReviewStatus())
			throw new ArgumentException(Messages.InvalidReviewStatus, nameof(status));
	}

	public static Review Create(
		Guid reviewerId,
		string status,
		DateTime date)
	{
		if (TryCreate(reviewerId, status, date, out Review? review, out string? errorMessage))
			return review!;

		throw new ArgumentException(errorMessage, nameof(Review));
	}

	public static bool TryCreate(
		Guid reviewerId,
		string status,
		DateTime date,
		out Review? review,
		out string? errorMessage)
	{
		review = null;
		errorMessage = null;

		if (reviewerId == Guid.Empty)
		{
			errorMessage = Messages.EnterReviewerId;
			return false;
		}

		if (!CheckInStatus.TryGet(status, out var statusVO, out errorMessage))
		{
			return false;
		}

		if (statusVO!.IsValidAsReviewStatus() == false)
		{
			errorMessage = Messages.InvalidReviewStatus;
			return false;
		}

		if (date < DateTime.MinValue || date > DateTime.MaxValue)
		{
			errorMessage = Messages.InvalidReviewDate;
			return false;
		}

		review = new Review(reviewerId, status, date);
		return true;
	}

	public override string ToString() => $"{ReviewerId}:{Status.Name}:{Date:O}";
}
