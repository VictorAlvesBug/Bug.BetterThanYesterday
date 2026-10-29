using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.CheckIns.GetChallengeUserDateWithCheckIns;

public class GetChallengeUserDateWithCheckInsCommand : ICommand
{
    public GetChallengeUserDateWithCheckInsCommand(
        Guid challengeId,
        Guid userId,
        DateTime date
    )
    {
        ChallengeId = challengeId;
        UserId = userId;
        Date = date;
    }
    
    public Guid ChallengeId { get; init; }
    public Guid UserId { get; init; }
    public DateTime Date { get; init; }
    
    public void Validate()
    {
        if (ChallengeId == Guid.Empty)
            throw new ArgumentNullException(nameof(ChallengeId), Messages.EnterChallengeId);
            
        if (UserId == Guid.Empty)
            throw new ArgumentNullException(nameof(UserId), Messages.EnterUserId);

        if (Date == DateTime.MinValue)
            throw new ArgumentNullException(nameof(Date), Messages.EnterCheckInDate);
    }
}