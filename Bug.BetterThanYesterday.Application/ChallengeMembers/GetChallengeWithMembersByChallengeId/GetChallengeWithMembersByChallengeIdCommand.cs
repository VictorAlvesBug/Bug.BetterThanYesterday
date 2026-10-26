using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.ChallengeMembers.GetChallengeWithMembersByChallengeId;

public class GetChallengeWithMembersByChallengeIdCommand : ICommand
{
    public GetChallengeWithMembersByChallengeIdCommand(Guid challengeId)
    {
        ChallengeId = challengeId;
    }

    public Guid ChallengeId { get; init; }

    public void Validate()
    {
        if (ChallengeId == Guid.Empty)
            throw new ArgumentNullException(nameof(ChallengeId), Messages.EnterChallengeId);
    }
}