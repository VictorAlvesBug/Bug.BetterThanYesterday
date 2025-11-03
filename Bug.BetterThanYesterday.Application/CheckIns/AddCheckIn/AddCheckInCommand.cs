using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.CheckIns.AddCheckIn;

public class AddCheckInCommand : ICommand
{
    public AddCheckInCommand(
        Guid planId,
        Guid userId,
        DateTime date,
        //int index,
        string title,
        string? description
    )
    {
        PlanId = planId;
        UserId = userId;
        Date = date;
        //Index = index;
        Title = title;
        Description = description;
    }
    
    public Guid PlanId { get; init; }
    public Guid UserId { get; init; }
    public DateTime Date { get; init; }
    //TODO Remover: public int Index { get; init; }
    public string Title { get; init; }
    public string? Description { get; init; }
    
    public void Validate()
    {
        if (PlanId == Guid.Empty)
            throw new ArgumentNullException(nameof(PlanId), Messages.PlanNotFound);
            
        if (UserId == Guid.Empty)
            throw new ArgumentNullException(nameof(UserId), Messages.UserNotFound);

        if (Date == DateTime.MinValue)
            throw new ArgumentException(nameof(Date), Messages.EnterCheckInDate);

        //if (Index < 0)
        //    throw new ArgumentException(nameof(Index), Messages.EnterCheckInIndex);

        if (string.IsNullOrWhiteSpace(Title))
            throw new ArgumentException(nameof(Title), Messages.EnterCheckInTitle);
    }
}