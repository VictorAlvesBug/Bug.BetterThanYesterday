using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.CheckIns.AddCheckIn;

public class AddCheckInCommand : ICommand
{
    public required Guid ChallengeId { get; init; }
    public required Guid UserId { get; init; }
    public required DateTime Date { get; init; }
    //TODO Adicionar: public int Index { get; init; }
    public required string Title { get; init; }
    public required string EvidencePhotoUrl { get; init; }

    public void Validate()
    {
        if (ChallengeId == Guid.Empty)
            throw new ArgumentNullException(nameof(ChallengeId), Messages.ChallengeNotFound);
            
        if (UserId == Guid.Empty)
            throw new ArgumentNullException(nameof(UserId), Messages.UserNotFound);

        if (Date == DateTime.MinValue)
            throw new ArgumentException(Messages.EnterCheckInDate, nameof(Date));

        //if (Index < 0)
        //    throw new ArgumentException(Messages.EnterCheckInIndex, nameof(Index));

        if (string.IsNullOrWhiteSpace(Title))
            throw new ArgumentException(Messages.EnterCheckInTitle, nameof(Title));

        if (string.IsNullOrWhiteSpace(EvidencePhotoUrl))
            throw new ArgumentException(Messages.EnterCheckInPhotoUrl, nameof(EvidencePhotoUrl));
    }
}
