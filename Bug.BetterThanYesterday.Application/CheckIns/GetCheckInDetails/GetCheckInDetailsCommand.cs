using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.CheckIns.GetCheckInDetails;

public class GetCheckInDetailsCommand : ICommand
{
    public GetCheckInDetailsCommand(
        Guid planId,
        Guid userId,
        DateTime date,
        int index
    )
    {
        PlanId = planId;
        UserId = userId;
        Date = date;
        Index = index;
    }
    
    public Guid PlanId { get; init; }
    public Guid UserId { get; init; }
    public DateTime Date { get; init; }
    public int Index { get; init; }
    
    public void Validate()
    {
        if (PlanId == Guid.Empty)
            throw new ArgumentNullException(nameof(PlanId), Messages.PlanNotFound);
            
        if (UserId == Guid.Empty)
            throw new ArgumentNullException(nameof(UserId), Messages.UserNotFound);

        if (Date == DateTime.MinValue)
            throw new ArgumentException(nameof(Date), Messages.EnterCheckInDate);

        if (Index < 0)
            throw new ArgumentException(nameof(Index), Messages.EnterCheckInIndex);
    }
}