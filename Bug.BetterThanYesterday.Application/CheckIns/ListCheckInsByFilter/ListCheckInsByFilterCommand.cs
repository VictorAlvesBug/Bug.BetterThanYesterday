using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.CheckIns.ListCheckInsByFilter;

public class ListCheckInsByFilterCommand : ICommand
{
    
    public Guid? PlanId { get; init; }
    public Guid? UserId { get; init; }
    public DateTime? Date { get; init; }
    public string? Status { get; init; }
    
	public void Validate()
    {
        if(PlanId is not null)
            return;

        if (UserId is not null || Date is not null || Status is not null)
            throw new ArgumentNullException(nameof(PlanId), Messages.ProvidePlanIdInOrderToUseAlsoOtherFilters);        
    }
}