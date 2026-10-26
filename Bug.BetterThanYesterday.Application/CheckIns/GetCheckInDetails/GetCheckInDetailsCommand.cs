using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.CheckIns.GetCheckInDetails;

public class GetCheckInDetailsCommand : ICommand
{
    public GetCheckInDetailsCommand(
        Guid challengeId,
        Guid userId,
        DateTime date,
        int index
    )
    {
        ChallengeId = challengeId;
        UserId = userId;
        Date = date;
        Index = index;
    }
    
    public Guid ChallengeId { get; init; }
    public Guid UserId { get; init; }
    public DateTime Date { get; init; }
    public int Index { get; init; }
    
    public void Validate()
    {
        if (ChallengeId == Guid.Empty)
            throw new ArgumentNullException(nameof(ChallengeId), Messages.ChallengeNotFound);
            
        if (UserId == Guid.Empty)
            throw new ArgumentNullException(nameof(UserId), Messages.UserNotFound);

        if (Date == DateTime.MinValue)
            throw new ArgumentException(Messages.EnterCheckInDate, nameof(Date));

        if (Index < 0)
            throw new ArgumentException(Messages.EnterCheckInIndex, nameof(Index));
    }
}