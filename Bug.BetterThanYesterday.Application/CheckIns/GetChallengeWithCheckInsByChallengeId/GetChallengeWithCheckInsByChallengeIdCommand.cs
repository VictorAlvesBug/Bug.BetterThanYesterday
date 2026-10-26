using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.CheckIns.GetChallengeWithCheckInsByChallengeId;

public class GetChallengeWithCheckInsByChallengeIdCommand : ICommand
{
    public GetChallengeWithCheckInsByChallengeIdCommand(Guid challengeId) => ChallengeId = challengeId;
    
    public Guid ChallengeId { get; init; }
    
    public void Validate()
    {
        if (ChallengeId == Guid.Empty)
            throw new ArgumentNullException(nameof(ChallengeId), Messages.EnterChallengeId);
    }
}