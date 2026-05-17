using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.CheckIns.GetCheckInById;

public class GetCheckInByIdCommand : ICommand
{
    public GetCheckInByIdCommand(Guid checkInId) => CheckInId = checkInId;

	public Guid CheckInId { get; init; }

	public void Validate()
	{
		if (CheckInId == Guid.Empty)
			throw new ArgumentNullException(nameof(CheckInId), Messages.EnterCheckInId);
	}
}