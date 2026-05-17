using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.CheckIns.AddCheckIn;

public class AddCheckInCommand : ICommand
{
    /*public AddCheckInCommand(
        Guid planId,
        Guid userId,
        DateTime date,
        //int index,
        string title,
        string photoUrl
    )
    {
        PlanId = planId;
        UserId = userId;
        Date = date;
        //Index = index;
        Title = title;
        PhotoUrl = photoUrl;
    }*/
    
    public required Guid PlanId { get; init; }
    public required Guid UserId { get; init; }
    public required DateTime Date { get; init; }
    //TODO Adicionar: public int Index { get; init; }
    public required string Title { get; init; }
    public required string PhotoUrl { get; init; }

    public void Validate()
    {
        if (PlanId == Guid.Empty)
            throw new ArgumentNullException(nameof(PlanId), Messages.PlanNotFound);
            
        if (UserId == Guid.Empty)
            throw new ArgumentNullException(nameof(UserId), Messages.UserNotFound);

        if (Date == DateTime.MinValue)
            throw new ArgumentException(Messages.EnterCheckInDate, nameof(Date));

        //if (Index < 0)
        //    throw new ArgumentException(Messages.EnterCheckInIndex, nameof(Index));

        if (string.IsNullOrWhiteSpace(Title))
            throw new ArgumentException(Messages.EnterCheckInTitle, nameof(Title));

        if (string.IsNullOrWhiteSpace(PhotoUrl))
            throw new ArgumentException(Messages.EnterCheckInPhotoUrl, nameof(PhotoUrl));
    }
}