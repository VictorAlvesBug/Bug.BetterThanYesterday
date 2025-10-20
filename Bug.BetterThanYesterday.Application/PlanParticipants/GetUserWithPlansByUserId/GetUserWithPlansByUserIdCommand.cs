using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.PlanParticipants.GetUserWithPlansByUserId;

public class GetUserWithPlansByUserIdCommand : ICommand
{
    public GetUserWithPlansByUserIdCommand(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; init; }

    public void Validate()
    {
        if (UserId == Guid.Empty)
            throw new ArgumentNullException(nameof(UserId), Messages.EnterUserId);
    }
}