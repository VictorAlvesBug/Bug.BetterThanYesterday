using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.CheckIns.AddCheckIn;

public class RemoveReviewCommand : ICommand
{   
    public required Guid CheckInId { get; init; }
    public required Guid ReviewerId { get; init; }

    public void Validate()
    {
        if (CheckInId == Guid.Empty)
            throw new ArgumentNullException(nameof(CheckInId), Messages.EnterCheckInId);
            
        if (ReviewerId == Guid.Empty)
            throw new ArgumentNullException(nameof(ReviewerId), Messages.EnterReviewerId);
    }
}