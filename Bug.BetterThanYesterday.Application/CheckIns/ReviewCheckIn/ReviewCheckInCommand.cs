using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.CheckIns.AddCheckIn;

public class ReviewCheckInCommand : ICommand
{   
    public required Guid CheckInId { get; init; }
    public required Guid ReviewerId { get; init; }
    public required string Status { get; init; }
    public required DateTime Date { get; init; }

    public void Validate()
    {
        if (CheckInId == Guid.Empty)
            throw new ArgumentNullException(nameof(CheckInId), Messages.EnterCheckInId);
            
        if (ReviewerId == Guid.Empty)
            throw new ArgumentNullException(nameof(ReviewerId), Messages.EnterReviewerId);

		if (string.IsNullOrWhiteSpace(Status))
			throw new ArgumentException(Messages.InvalidReviewStatus, nameof(Status));

		if (Date == DateTime.MinValue)
			throw new ArgumentException(Messages.EnterCheckInDate, nameof(Date));
    }
}