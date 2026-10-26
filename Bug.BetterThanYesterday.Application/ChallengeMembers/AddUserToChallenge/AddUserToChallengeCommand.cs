using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.ChallengeMembers.AddUserToChallenge;
public class AddUserToChallengeCommand : ICommand
{
    public AddUserToChallengeCommand(Guid challengeId, Guid userId)
    {
        ChallengeId = challengeId;
        UserId = userId;
    }

    public Guid ChallengeId { get; init; }
    public Guid UserId { get; init; }

    public void Validate()
    {
        if (ChallengeId == Guid.Empty)
            throw new ArgumentNullException(nameof(ChallengeId), Messages.EnterChallengeId);
        
        if (UserId == Guid.Empty)
            throw new ArgumentNullException(nameof(UserId), Messages.EnterUserId);
    }
}