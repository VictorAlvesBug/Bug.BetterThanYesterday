using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.CheckIns.GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDate;

public class GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDateCommand : ICommand
{
    public GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDateCommand(
        Guid planId,
        Guid userId,
        DateTime date
    )
    {
        PlanId = planId;
        UserId = userId;
        Date = date;
    }
    
    public Guid PlanId { get; init; }
    public Guid UserId { get; init; }
    public DateTime Date { get; init; }
    
    public void Validate()
    {
        if (PlanId == Guid.Empty)
            throw new ArgumentNullException(nameof(PlanId), Messages.EnterPlanId);
            
        if (UserId == Guid.Empty)
            throw new ArgumentNullException(nameof(UserId), Messages.EnterUserId);

        if (Date == DateTime.MinValue)
            throw new ArgumentNullException(nameof(Date), Messages.EnterCheckInDate);
    }
}