using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.AdminSettings.MoveInTime;

public class MoveInTimeCommand : ICommand
{
	public int DaysAmount { get; init; }

	public void Validate()
	{
		if (DaysAmount == 0)
			throw new ArgumentNullException(nameof(DaysAmount), Messages.EnterDaysAmountToMoveInTime);
	}
}
