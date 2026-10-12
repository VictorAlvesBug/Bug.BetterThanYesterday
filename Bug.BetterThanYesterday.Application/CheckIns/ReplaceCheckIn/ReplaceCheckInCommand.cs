using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.CheckIns.ReplaceCheckIn;

public class ReplaceCheckInCommand : ICommand
{
	public required Guid CheckInId { get; init; }
	public required Guid UserId { get; init; }
	public required string Title { get; init; }
	public required string EvidencePhotoUrl { get; init; }

	public void Validate()
	{
		if (CheckInId == Guid.Empty)
			throw new ArgumentNullException(nameof(CheckInId), Messages.EnterCheckInId);

		if (UserId == Guid.Empty)
			throw new ArgumentNullException(nameof(UserId), Messages.EnterUserId);

		if (string.IsNullOrWhiteSpace(Title))
			throw new ArgumentException(Messages.EnterCheckInTitle, nameof(Title));

		if (string.IsNullOrWhiteSpace(EvidencePhotoUrl))
			throw new ArgumentException(Messages.EnterCheckInPhotoUrl, nameof(EvidencePhotoUrl));
	}
}
